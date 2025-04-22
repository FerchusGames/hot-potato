using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    [CustomEditor(typeof(WatchGeneratorSO))]
    [CanEditMultipleObjects]
    public class WatchGeneratorSO_Editor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var watchGenerator = (WatchGeneratorSO)target;
            var watchGenerators = targets.Select(t => t as WatchGeneratorSO).ToArray();
            var autoRegenerateProperty = serializedObject.FindProperty(nameof(WatchGeneratorSO.AutoRegenOnChange));
            var schemeClassProperty = serializedObject.FindProperty(nameof(WatchGeneratorSO.SchemaClassFile));
            var outputClassProperty = serializedObject.FindProperty(nameof(WatchGeneratorSO.OutputClassFile));
            var outputNameProperty = serializedObject.FindProperty(nameof(WatchGeneratorSO.OutputClassName));
            var outputNamespaceProperty = serializedObject.FindProperty(nameof(WatchGeneratorSO.OutputNamespaceName));
            var outputSettingsProperty = serializedObject.FindProperty(nameof(WatchGeneratorSO.OutputClassSettings));
            var generatorSettingsProperty = serializedObject.FindProperty(nameof(WatchGeneratorSO.GeneratorSettings));

            EditorGUILayout.PropertyField(autoRegenerateProperty);
            EditorGUILayout.PropertyField(outputNameProperty);
            EditorGUILayout.PropertyField(outputNamespaceProperty);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(schemeClassProperty);
            
            if (watchGenerator.SchemaClassFile == null)
            {
                if (GUILayout.Button("Create file"))
                    WatchGenerationManager.CreateDraftSchemeClassFile(watchGenerator);
            }
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(outputClassProperty);
            
            if (watchGenerator.OutputClassFile == null)
            {
                if (GUILayout.Button("Create file"))
                    WatchGenerationManager.CreateDraftOutputClassFile(watchGenerator);
            }
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.PropertyField(outputSettingsProperty);
            EditorGUILayout.PropertyField(generatorSettingsProperty);
            
            EditorGUILayout.Space();

            if (GUILayout.Button("Generate", GUILayout.Height(50)))
            {
                foreach (var generator in watchGenerators)
                {
                    WatchGenerationManager.GenerateWatchesFromSO(generator);
                }
            }

            if (GUILayout.Button("Generate empty"))
            {
                foreach (var generator in watchGenerators)
                {
                    WatchGenerationManager.GenerateEmptyWatchesFromSO(generator);
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("If you have compiler issues because of generated watches, press GenerateEmpty, then after successful recompilation press Generate", MessageType.Info);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}