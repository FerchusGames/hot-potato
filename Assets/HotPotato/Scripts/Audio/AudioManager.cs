using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using HotPotato.Utilities;
using Sirenix.OdinInspector;

namespace HotPotato.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("Volume")]
        [Range(0, 1)]
        public float masterVolume = 1;
        [Range(0, 1)]
        public float musicVolume = 1;
        [Range(0, 1)]
        public float ambienceVolume = 1;
        [Range(0, 1)]
        public float SFXVolume = 1;

        [Required]
        [SerializeField] private  EventReferenceSO _ambienceEventReference;
        
        [Required]
        [SerializeField] private EventReferenceSO _musicEventReference;
        
        private Bus _masterBus;
        private Bus _musicBus;
        private Bus _ambienceBus;
        private Bus _sfxBus;

        private List<EventInstance> _eventInstances;
        private List<StudioEventEmitter> _eventEmitters;

        private EventInstance _ambienceEventInstance;
        private EventInstance _musicEventInstance;
        
        private void Awake()
        {
            _eventInstances = new List<EventInstance>();
            _eventEmitters = new List<StudioEventEmitter>();

            _masterBus = RuntimeManager.GetBus("bus:/");
            _musicBus = RuntimeManager.GetBus("bus:/Music");
            _ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
            _sfxBus = RuntimeManager.GetBus("bus:/SFX");
        }

        private void Start()
        {
            InitializeAmbience(_ambienceEventReference.EventReference);
            InitializeMusic(_musicEventReference.EventReference);
        }

        private void Update()
        {
            _masterBus.setVolume(masterVolume);
            _musicBus.setVolume(musicVolume);
            _ambienceBus.setVolume(ambienceVolume);
            _sfxBus.setVolume(SFXVolume);
        }

        private void InitializeAmbience(EventReference ambienceEventReference)
        {
            _ambienceEventInstance = CreateInstance(ambienceEventReference);
            _ambienceEventInstance.start();
        }

        private void InitializeMusic(EventReference musicEventReference)
        {
            _musicEventInstance = CreateInstance(musicEventReference);
            _musicEventInstance.start();
        }

        public void SetAmbienceParameter(string parameterName, float parameterValue)
        {
            _ambienceEventInstance.setParameterByName(parameterName, parameterValue);
        }

        public void SetMusicTrack(MusicTrack track)
        {
            _musicEventInstance.setParameterByName("track", (float) track);
        }

        public void PlayOneShot(EventReference sound, Vector3 worldPos)
        {
            RuntimeManager.PlayOneShot(sound, worldPos);
        }

        public EventInstance CreateInstance(EventReference eventReference)
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
            _eventInstances.Add(eventInstance);
            return eventInstance;
        }

        public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, StudioEventEmitter studioEventEmitter)
        {
            studioEventEmitter.EventReference = eventReference;
            _eventEmitters.Add(studioEventEmitter);
            return studioEventEmitter;
        }

        private void CleanUp()
        {
            // stop and release any created instances
            foreach (EventInstance eventInstance in _eventInstances)
            {
                eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                eventInstance.release();
            }
            // stop all of the event emitters, because if we don't they may hang around in other scenes
            foreach (StudioEventEmitter emitter in _eventEmitters)
            {
                emitter.Stop();
            }
        }

        private void OnDestroy()
        {
            CleanUp();
        }
    }
}