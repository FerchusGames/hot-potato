using System;
using System.Runtime.InteropServices;
using Dissonance.Audio.Playback;
using FMOD;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dissonance.Integrations.FMOD_Playback
{
    public class FMODVoicePlayback
        : BaseVoicePlayback
    {
        private static readonly Log Log = Logs.Create(LogCategory.Playback, nameof(FMODVoicePlayback));

        private AudioGenerator _generator;
        private GCHandle _handle;
        private DSP _dsp;
        private Channel _channel;
        private int _sampleRate;
        private FMODChannelGroupLocks.Handle? _busLock;

        public override float Amplitude => _generator?.Amplitude ?? 0;

        /// <summary>
        /// Indicates if this playback object has been virtualised by FMOD - if true then no audio will be heard!
        /// </summary>
        public bool IsVirtualised => _generator?.IsVirtual ?? false;

        [SerializeField, Tooltip("Audio Minimum distance")] public float MinDistance = 5;
        [SerializeField, Tooltip("Audio Maximum distance")] public float MaxDistance = 100;
        [SerializeField, Tooltip("Audio Attenuation Mode")] public RolloffMode RolloffMode = RolloffMode.InverseTapered;
        [SerializeField, Tooltip("Output Audio Bus")] public string OutputBusID = null;

        [SerializeField, Tooltip("Disable passing positional information to FMOD"), FormerlySerializedAs("DisablePositionalAudio")] private bool _disablePositionalAudio;
        /// <summary>
        /// Disable passing positional information to FMOD, forcing audio to non-spatialized. This Overrides the `Use Positional Data` setting in the broadcast trigger.
        /// </summary>
        public bool DisablePositionalAudio
        {
            get => _disablePositionalAudio;
            set
            {
                // Don't make any changes unless necessary
                if (value == _disablePositionalAudio)
                    return;
                _disablePositionalAudio = value;

                // If there's an active speech session, immediately apply this change
                if (LatestPlaybackOptions.HasValue)
                    UpdatePositionalPlayback(LatestPlaybackOptions.Value);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            // Get the sample rate which audio must be produced at
            RuntimeManager.CoreSystem.getSoftwareFormat(out _sampleRate, out _, out _);

            // Create an `AudioGenerator` which handles DSP logic for a specific Dissonance
            // playback pipeline.
            _generator = new AudioGenerator(this);
            _handle = GCHandle.Alloc(_generator);

            // Create the DSP object in FMOD
            // - No input buffers (not needed, this is a generator)
            // - `ReadDSP` method pulls data from Dissonance
            // - `ShouldProcessDSP` informs FMOD when there is no audio available
            // - `userdata` stashes a GCHandle to the AudioGenerator object
            var desc = new DSP_DESCRIPTION
            {
                numinputbuffers = 0,
                numoutputbuffers = 1,
                read = ReadDSP,
                shouldiprocess = ShouldProcessDSP,
                userdata = (IntPtr)_handle,
            };
            RuntimeManager.CoreSystem.createDSP(ref desc, out _dsp);

            // Start the DSP playing - it will play all of the time that this GameObject is enabled.
            // i.e. all the time that this speaker is connected to the Dissonance session.
            _busLock = FMODChannelGroupLocks.Instance.LockBus(OutputBusID);
            RuntimeManager.CoreSystem.playDSP(_dsp, _busLock?.ChannelGroup ?? default, false, out _channel);

            // Set the callback handler which will detect when the channel becomes virtual
            _channel.setUserData((IntPtr)_handle);
            _channel.setCallback(ChannelEventCallback);
        }

        private static bool Check(RESULT result, string message, bool err = false)
        {
            if (result == RESULT.OK)
                return true;

            const string fmt = "{0}. FMOD Result: {1}";
            if (err)
                Log.Error(fmt, message, result);
            else
                Log.Warn(fmt, message, result);

            return false;

        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Teardown();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Teardown();
        }

        private void Teardown()
        {
            Log.Debug("Teardown FMOD playback");

            if (_channel.hasHandle())
            {
                _channel.stop();
                _channel.setUserData(IntPtr.Zero);
                _channel.removeDSP(_dsp);
                _channel.clearHandle();
            }

            if (_dsp.hasHandle())
            {
                _dsp.setUserData(IntPtr.Zero);
                _dsp.release();
                _dsp.clearHandle();
            }

            if (_busLock.HasValue)
            {
                FMODChannelGroupLocks.Instance.UnlockBus(_busLock.Value);
                _busLock = default;
            }

            if (_handle.IsAllocated)
                _handle.Free();
        }

        protected override void Update()
        {
            base.Update();

            var active = _generator.ActiveSession;
            if (!active.HasValue)
            {
                // We're not playing anything at the moment. Try to get a session to play.
                var s = TryDequeueSession(_sampleRate);
                if (s.HasValue)
                    _generator.Start(s.Value);
            }

            UpdateOutputBus();
            if (LatestPlaybackOptions.HasValue)
                UpdatePositionalPlayback(LatestPlaybackOptions.Value);
        }

        private void FixedUpdate()
        {
            // If FMOD is not playing audio because the channel has been virtualised then
            // we need to pump the channel for audio ourselves to stay in sync. This call
            // will only do somethnig if the generator has been set to virtualised, which is
            // handled by a callback from FMOD on the virtual<->real transition (see: ChannelEventCallback)
            _generator.PumpVirtualised(Time.fixedUnscaledDeltaTime, _sampleRate);
        }

        private void UpdateOutputBus()
        {
            // If there's already no lock (i.e. it's not currently set to a valid bus) and it's
            // being set to null or whitespace (i.e. not a valid bus) don't make any changes.
            if (!_busLock.HasValue && string.IsNullOrWhiteSpace(OutputBusID))
                return;

            if (_busLock?.Name != OutputBusID)
            {
                Log.Trace("Changing output bus from `{0}` to `{1}`", _busLock?.Name, OutputBusID);

                // Release the old lock
                if (_busLock.HasValue)
                    FMODChannelGroupLocks.Instance.UnlockBus(_busLock.Value);

                // Lock the new channel
                _busLock = FMODChannelGroupLocks.Instance.LockBus(OutputBusID);

                // Attempt to set the channel, give up if it fails
                if (!Check(_channel.setChannelGroup(_busLock?.ChannelGroup ?? default), "Failed to change channel group"))
                    return;
            }
        }

        private void UpdatePositionalPlayback(PlaybackOptions options)
        {
            if (!_channel.hasHandle())
                return;

            // Set 2D/3D mode into FMOD as appropriate
            Check(_channel.getMode(out var mode), "Failed `getMode`", true);
            var isPositional = options.IsPositional;
            var allowPosition = ((IVoicePlaybackInternal)this).AllowPositionalPlayback && !DisablePositionalAudio;
            if (allowPosition && isPositional)
            {
                if ((mode & MODE._2D) == MODE._2D || (mode & (MODE)RolloffMode) != (MODE)RolloffMode)
                    Check(_channel.setMode(MODE._3D | (MODE)RolloffMode), "Failed `setMode`", true);
                Check(_channel.set3DMinMaxDistance(MinDistance, MaxDistance), "Failed `set3DMinMaxDistance`", true);
            }
            else
            {
                if ((mode & MODE._2D) != MODE._2D)
                    Check(_channel.setMode(MODE._2D), "Failed `setMode`", true);
            }
        }

        protected override void SetTransform(Vector3 pos, Quaternion rot)
        {
            base.SetTransform(pos, rot);

            if (!_channel.hasHandle())
                return;

            Check(_channel.getMode(out var mode), "Failed `getMode`", true);
            if ((mode & MODE._3D) == MODE._3D)
            {
                var p = new VECTOR { x = pos.x, y = pos.y, z = pos.z };
                var v = new VECTOR();
                Check(_channel.set3DAttributes(ref p, ref v), "Failed `set3DAttributes`", true);
            }
        }

        protected override SpeechSession? TryGetActiveSession()
        {
            return _generator?.ActiveSession;
        }

        #region DSP
        [AOT.MonoPInvokeCallback(typeof(CHANNELCONTROL_CALLBACK))]
        private static RESULT ChannelEventCallback(IntPtr channelcontrol, CHANNELCONTROL_TYPE controltype, CHANNELCONTROL_CALLBACK_TYPE callbacktype, IntPtr commanddata1, IntPtr commanddata2)
        {
            if (controltype != CHANNELCONTROL_TYPE.CHANNEL)
                return RESULT.OK;
            if (callbacktype != CHANNELCONTROL_CALLBACK_TYPE.VIRTUALVOICE)
                return RESULT.OK;

            // Get user data from channel. If this fails something has gone horribly wrong and there's nothing we can do about it.
            var channel = new Channel(channelcontrol);
            if (!Check(channel.getUserData(out var userdata), "Failed to getUserData from channel"))
                return RESULT.OK;

            // Userdata may be null if this is called after teardown
            if (userdata == IntPtr.Zero)
                return RESULT.OK;

            // Extract a data source from the GC handle
            var ud = GCHandle.FromIntPtr(userdata);
            var tgt = (AudioGenerator)ud.Target;

            // If the target is null just early exit
            if (tgt == null)
                return RESULT.OK;

            // Set generator to virtualised as necessary
            // 0 -> 'virtual to real'
            // 1 -> 'real to virtual'.
            tgt.SetVirtualised(commanddata1.ToInt32() != 0);

            return RESULT.OK;
        }

        [AOT.MonoPInvokeCallback(typeof(DSP_READ_CALLBACK))]
        private static RESULT ReadDSP(ref DSP_STATE dsp_state, IntPtr inbuffer, IntPtr outbuffer, uint length, int inchannels, ref int outchannels)
        {
            // FMOD 2.02: var functions = (DSP_STATE_FUNCTIONS)Marshal.PtrToStructure(dsp_state.functions, typeof(DSP_STATE_FUNCTIONS));
            var functions = dsp_state.functions;

            // Get the user data, this has a stashed refernce to the audio data source
            var gudResult = functions.getuserdata(ref dsp_state, out var userdata);
            if (gudResult != RESULT.OK || userdata == IntPtr.Zero)
            {
                FloatMemClear(outbuffer, length);
                return RESULT.OK;
            }

            // Extract a data source from the GC handle and copy the data into the DSP output buffer
            var ud = GCHandle.FromIntPtr(userdata);
            var tgt = (AudioGenerator)ud.Target;

            // If the target is null clear the buffer to silence and early exit.
            if (tgt == null)
            {
                FloatMemClear(outbuffer, length);
                return RESULT.OK;
            }

            return tgt.GetAudio(outbuffer, length, (uint)outchannels);
        }

        [AOT.MonoPInvokeCallback(typeof(DSP_SHOULDIPROCESS_CALLBACK))]
        private static RESULT ShouldProcessDSP(ref DSP_STATE dsp_state, bool inputsidle, uint length, CHANNELMASK inmask, int inchannels, SPEAKERMODE speakermode)
        {
            // Get the user data, this has a stashed refernce to the audio data source
            // FMOD 2.02 var functions = (DSP_STATE_FUNCTIONS)Marshal.PtrToStructure(dsp_state.functions, typeof(DSP_STATE_FUNCTIONS));
            var functions = dsp_state.functions;

            var gudResult = functions.getuserdata(ref dsp_state, out var userdata);
            if (gudResult != RESULT.OK || userdata == IntPtr.Zero)
                return RESULT.OK;

            // Get the generator and see if it wants processing
            var ud = GCHandle.FromIntPtr(userdata);
            var tgt = (AudioGenerator)ud.Target;

            // If the target is null just early exit and return a code indicating this DSP is silent
            if (tgt == null)
                return RESULT.ERR_DSP_SILENCE;

            return tgt.ShouldProcess();
        }

        private static void FloatMemClear(IntPtr buffer, uint length)
        {
            unsafe
            {
                var ptr = (float*) buffer.ToPointer();
                FloatMemClear(ptr, length);
            }
        }

        private static unsafe void FloatMemClear(float* buffer, uint length)
        {
            for (var i = 0; i < length; i++)
                buffer[i] = 0;
        }

        /// <summary>
        /// Pulls audio from a `SpeechSession` into a buffer, suitable for use in FMOD.
        /// </summary>
        private class AudioGenerator
        {
            private readonly FMODVoicePlayback _parent;

            public float Amplitude { get; private set; }

            private SpeechSession? _session;
            public SpeechSession? ActiveSession => _session;

            private float[] _temp;

            private readonly object _pumpLock = new object();
            private volatile bool _isVirtual;
            private float _virtualisedTimeAccumulator;

            public bool IsVirtual => _isVirtual;

            public AudioGenerator(FMODVoicePlayback parent)
            {
                _parent = parent;
            }

            public RESULT GetAudio(IntPtr outbuffer, uint length, uint outchannels)
            {
                lock (_pumpLock)
                {
                    unsafe
                    {
                        // If there is no active session output silence.
                        var session = ActiveSession;
                        if (!session.HasValue)
                        {
                            if (outbuffer != IntPtr.Zero)
                                FloatMemClear((float*)outbuffer.ToPointer(), length);
                            return RESULT.OK;
                        }

                        // Expand the temp buffer if necessary. Oversize it so
                        // that we have to resize it less often.
                        var samplesRequired = length / outchannels;
                        if (_temp == null || samplesRequired > _temp.Length)
                            _temp = new float[samplesRequired * 2];

                        // Read that much data from the voice session
                        var complete = session.Value.Read(new ArraySegment<float>(_temp, 0, (int)samplesRequired));
                        if (complete)
                            _session = null;

                        // Step through samples, stretching them (i.e. play mono input in all output channels)
                        // Skip this step if the output buffer is null (i.e. caller doesn't actually care about the data).
                        float accumulator = 0;
                        if (outbuffer == IntPtr.Zero)
                        {
                            var sampleIndex = 0;
                            for (var i = 0u; i < length; i += outchannels)
                                accumulator += Mathf.Abs(_temp[sampleIndex++]);
                        }
                        else
                        {
                            var output = (float*)outbuffer.ToPointer();
                            var sampleIndex = 0;
                            for (var i = 0u; i < length; i += outchannels)
                            {
                                //Get a single sample from the source data
                                var sample = _temp[sampleIndex++];

                                //Accumulate the sum of the rectified audio signal
                                accumulator += Mathf.Abs(sample);

                                //Copy data into all channels
                                for (var c = 0; c < outchannels; c++)
                                    output[i + c] = sample;
                            }
                        }

                        // Output the final ARV amplitude
                        Amplitude = accumulator / length;
                    }

                    return RESULT.OK;
                }
            }

            public RESULT ShouldProcess()
            {
                // Returning `ERR_DSP_SILENCE` informs FMOD that the output of this DSP is silent and
                // it can be safely ignored.
                return _session.HasValue
                     ? RESULT.OK
                     : RESULT.ERR_DSP_SILENCE;
            }

            public void Start(SpeechSession speechSession)
            {
                if (_session != null)
                    throw new InvalidOperationException("Cannot start a new voice session when one is already playing");

                _session = speechSession;
                _parent.UpdatePositionalPlayback(speechSession.PlaybackOptions);
            }

            public void PumpVirtualised(float deltaTime, int sampleRate)
            {
                // ReSharper disable once InconsistentlySynchronizedField
                // Double checked locking
                if (!_isVirtual)
                    return;

                lock (_pumpLock)
                {
                    if (!_isVirtual)
                        return;

                    // Accumulate time elapsed since this audio generator was made virtual
                    _virtualisedTimeAccumulator += deltaTime;

                    // Take as many complete integer samples as possible
                    var samples = (uint)Math.Floor(_virtualisedTimeAccumulator * sampleRate);

                    // Decrease the timer by however much time that many samples is
                    _virtualisedTimeAccumulator -= (float)samples / sampleRate;

                    // Pump for that much audio, passing a null pointer because we don't
                    // actually care about the result
                    GetAudio(IntPtr.Zero, samples, 1);
                }
            }

            public void SetVirtualised(bool virtualised)
            {
                Log.Debug("SetVirtualised({0})", virtualised);

                lock (_pumpLock)
                {
                    _isVirtual = virtualised;
                    _virtualisedTimeAccumulator = 0;
                }
            }
        }
        #endregion
    }
}

