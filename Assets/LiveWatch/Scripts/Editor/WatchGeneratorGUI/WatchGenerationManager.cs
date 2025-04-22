using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ingvar.LiveWatch.Generation;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    internal static class WatchGenerationManager
    {
        [InitializeOnLoadMethod]
        private static void OnUnityReloaded()
        {
            var generators = AssetExtensions.FindAssetsByType<WatchGeneratorSO>();

            foreach (var generator in generators)
            {
                if (generator.SchemaClassFile == null)
                {
                    continue;
                }

                if (generator.PreviousOutputHashcode == 0)
                {
                    generator.PreviousOutputHashcode = generator.SchemaClassFile.text.GetHashCode();
                    AssetExtensions.SaveAndRefresh(generator);
                    continue;
                }

                if (generator.OutputClassFile == null || !generator.AutoRegenOnChange)
                {
                    continue;
                }
                
                if (generator.PreviousOutputHashcode == generator.SchemaClassFile.text.GetHashCode())
                {
                    continue;
                }

                GenerateWatchesFromSO(generator);
            }
        }

        public static void GenerateAll()
        {
            var guids = AssetDatabase.FindAssets($"t: {nameof(WatchGeneratorSO)}");

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var generator = AssetDatabase.LoadAssetAtPath<WatchGeneratorSO>(path);
                
                if (generator != null)
                    GenerateWatchesFromSO(generator);
            }
        }
        
        public static void GenerateEmptyAll()
        {
            var guids = AssetDatabase.FindAssets($"t: {nameof(WatchGeneratorSO)}");

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var generator = AssetDatabase.LoadAssetAtPath<WatchGeneratorSO>(path);
                
                if (generator != null)
                    GenerateEmptyWatchesFromSO(generator);
            }
        }
        
        internal static void CreateDraftSchemeClassFile(WatchGeneratorSO watchGeneratorSO)
        {
            if (string.IsNullOrWhiteSpace(watchGeneratorSO.OutputClassName))
            {
                Debug.LogError("Please provide a name for an output class so we can use it for schema name!");
                return;
            }
            
            var soPath = AssetDatabase.GetAssetPath(watchGeneratorSO);
            var schemaName = $"{watchGeneratorSO.OutputClassName}Schema";
            var schemaFolderPath = Path.GetDirectoryName(soPath) ?? string.Empty;
            var schemaPath = Path.Combine(schemaFolderPath, $"{schemaName}.cs");
            var schemaContent = GetDraftSchemaContent(schemaName);
            File.WriteAllText(schemaPath, schemaContent);
            
            AssetDatabase.Refresh();
            var schemaTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(schemaPath);
            
            watchGeneratorSO.SchemaClassFile = schemaTextAsset;
            AssetExtensions.SaveAndRefresh(watchGeneratorSO);
        }

        internal static void CreateDraftOutputClassFile(WatchGeneratorSO watchGeneratorSO)
        {
            if (string.IsNullOrWhiteSpace(watchGeneratorSO.OutputClassName))
            {
                Debug.LogError("Please provide a name for an output class!");
                return;
            }
            
            var soPath = AssetDatabase.GetAssetPath(watchGeneratorSO);
            var schemaFolderPath = Path.GetDirectoryName(soPath) ?? string.Empty;
            var outputClassPath = Path.Combine(schemaFolderPath, $"{watchGeneratorSO.OutputClassName}.cs");
            
            WatchEditorServices.Generator.GenerateFromDescriptorsToPath(
                outputClassPath,
                watchGeneratorSO.OutputClassName, 
                watchGeneratorSO.OutputNamespaceName,
                watchGeneratorSO.OutputClassSettings,
                watchGeneratorSO.GeneratorSettings,
                null);
            
            AssetDatabase.Refresh();
            var outputTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(outputClassPath);
            
            watchGeneratorSO.OutputClassFile = outputTextAsset;
            AssetExtensions.SaveAndRefresh(watchGeneratorSO);
        }

        internal static void GenerateEmptyWatchesFromSO(WatchGeneratorSO watchGeneratorSO)
        {
            if (!ValidateSO(watchGeneratorSO))
                return;
            
            var generationSchema = WatchEditorServices.GenerationSchemaParser.ReadSchema(watchGeneratorSO.SchemaClassFile.text);

            if (generationSchema == null)
            {
                Debug.LogError($"Failed to read generation scheme with name: {watchGeneratorSO.SchemaClassFile.name}! " +
                               $"Make sure it's valid and inherited from {nameof(WatchGenerationSchema)}");
                return;
            }
            
            var outputClassPath = AssetDatabase.GetAssetPath(watchGeneratorSO.OutputClassFile);

            WatchGenerator.DontGenerateChildCode = true;
            WatchEditorServices.Generator.GenerateFromDescriptorsToPath(
                outputClassPath,
                watchGeneratorSO.OutputClassName, 
                watchGeneratorSO.OutputNamespaceName,
                watchGeneratorSO.OutputClassSettings,
                watchGeneratorSO.GeneratorSettings,
                generationSchema);
                
            AssetExtensions.SaveAndRefresh(watchGeneratorSO);
        }
        
        internal static void GenerateWatchesFromSO(WatchGeneratorSO watchGeneratorSO)
        {
            if (!ValidateSO(watchGeneratorSO))
                return;
            
            watchGeneratorSO.PreviousOutputHashcode = watchGeneratorSO.SchemaClassFile.text.GetHashCode();
                
            var generationSchema = WatchEditorServices.GenerationSchemaParser.ReadSchema(watchGeneratorSO.SchemaClassFile.text);

            if (generationSchema == null)
            {
                Debug.LogError($"Failed to read generation scheme with name: {watchGeneratorSO.SchemaClassFile.name}! " +
                               $"Make sure it's valid and inherited from {nameof(WatchGenerationSchema)}");
                return;
            }
            
            var outputClassPath = AssetDatabase.GetAssetPath(watchGeneratorSO.OutputClassFile);
            
            WatchGenerator.DontGenerateChildCode = false;
            WatchEditorServices.Generator.GenerateFromDescriptorsToPath(
                outputClassPath,
                watchGeneratorSO.OutputClassName, 
                watchGeneratorSO.OutputNamespaceName,
                watchGeneratorSO.OutputClassSettings,
                watchGeneratorSO.GeneratorSettings,
                generationSchema);
                
            AssetExtensions.SaveAndRefresh(watchGeneratorSO);
        }

        private static string GetDraftSchemaContent(string schemaName)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"using Ingvar.LiveWatch.Generation;");
            sb.AppendLine();
            sb.AppendLine($"public class {schemaName} : WatchGenerationSchema");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic override void OnGenerate()");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\t");
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            return sb.ToString();
        }
        
        private static bool ValidateSO(WatchGeneratorSO watchGeneratorSO)
        {
            if (string.IsNullOrWhiteSpace(watchGeneratorSO.OutputClassName))
            {
                Debug.LogError("Please provide a valid name for an output class!");
                return false;
            }
            
            if (watchGeneratorSO.SchemaClassFile == null)
            {
                Debug.LogError("Please provide a reference to a scheme class file!");
                return false;
            }
            
            if (watchGeneratorSO.OutputClassFile == null)
            {
                Debug.LogError("Please provide a reference to an output class file!");
                return false;
            }

            return true;
        }
    }
}