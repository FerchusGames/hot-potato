using System;
using Dissonance.Editor;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEditor;
using UnityEngine;
using GUID = FMOD.GUID;

namespace Dissonance.Integrations.FMOD_Playback.Editor
{
    [CustomEditor(typeof(FMODVoicePlayback))]
    [CanEditMultipleObjects]
    public class FMODVoicePlaybackEditor
        : BaseVoicePlaybackEditor<FMODVoicePlayback>
    {
        private string _typingBusName;
        private string _busError;
        private FMOD.Studio.System? _system;

        private void OnEnable()
        {
            _system = EditorUtils.System;
            EditorUtils.LoadPreviewBanks();
        }

        private void OnDisable()
        {
            _system = null;
        }

        protected override void OnGuiTop()
        {
            var busProperty = serializedObject.FindProperty(nameof(FMODVoicePlayback.OutputBusID));

            if (_typingBusName == null && !string.IsNullOrEmpty(busProperty.stringValue))
                _typingBusName = BusName(busProperty.stringValue);

            var min = serializedObject.FindProperty(nameof(FMODVoicePlayback.MinDistance));
            EditorGUILayout.PropertyField(min);

            var max = serializedObject.FindProperty(nameof(FMODVoicePlayback.MaxDistance));
            EditorGUILayout.PropertyField(max);

            BusUI(busProperty);

            var rolloff = serializedObject.FindProperty(nameof(FMODVoicePlayback.RolloffMode));
            EditorGUILayout.PropertyField(rolloff);

            var disablePositional = serializedObject.FindProperty("_disablePositionalAudio");
            EditorGUILayout.PropertyField(disablePositional);

            base.OnGuiTop();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying)
            {
                EditorGUILayout.LabelField("FMOD Virtualised", ((FMODVoicePlayback)target).IsVirtualised.ToString());
            }
        }

        private void BusUI(SerializedProperty busID)
        {
            var label = new GUIContent("FMOD Bus");

            // Get text from the user, don't do anything if it hasn't changed
            var typingInput = _typingBusName;
            var typingOutput = EditorGUILayout.TextField(label, typingInput);
            if (typingInput != typingOutput)
            {
                _typingBusName = typingOutput;
                _busError = null;

                // This might be a valid bus (update property), or it might be an error (store error string)
                var idOrErr = BusID(_typingBusName, out var err);
                if (idOrErr != null && !err)
                    busID.stringValue = idOrErr;
                else
                    _busError = idOrErr;
                
            }

            // Show error string if there is one
            if (!string.IsNullOrWhiteSpace(_busError))
                EditorGUILayout.HelpBox(_busError, MessageType.Error);

            EditorGUILayout.LabelField("FMOD Bus ID", busID.stringValue);
        }

        private string BusID(string name, out bool err)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                err = false;
                return "";
            }

            var bus = default(Bus);
            var result = _system?.getBus(name, out bus) ?? RESULT.ERR_STUDIO_UNINITIALIZED;
            if (result != RESULT.OK)
            {
                err = true;
                return $"FMOD Error: {result}";
            }

            var result2 = bus.getID(out var id);
            if (result2 != RESULT.OK)
            {
                err = true;
                return $"FMOD Error: {result2}";
            }

            err = false;
            return id.ToString();
        }

        private string BusName(string id)
        {
            GUID guid;
            try
            {
                guid = GUID.Parse(id);
            }
            catch (FormatException)
            {
                return $"Invalid GUID format: `{id}`";
            }

            var bus = default(Bus);
            var result = _system?.getBusByID(guid, out bus) ?? RESULT.ERR_STUDIO_UNINITIALIZED;
            if (result != RESULT.OK)
                return $"FMOD Error: `{result}`";

            var result2 = bus.getPath(out var path);
            if (result2 != RESULT.OK)
                return $"FMOD Error: `{result2}`";

            return path;
        }
    }
}