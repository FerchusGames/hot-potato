using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Ingvar.LiveWatch.Editor;
using UnityEngine;
using Descriptor = Ingvar.LiveWatch.Generation.WatchVariableDescriptorUtils;
using Schema = Ingvar.LiveWatch.Generation.WatchGenerationSchemaUtils;

namespace Ingvar.LiveWatch.Generation
{
    public partial class WatchGenerator
    {
        public static string ScriptingDefineWithBuild = "UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD";
        public static string NullValue = "NULL";
        public static bool DontGenerateChildCode;
        public bool GenerateCountForCollections => _generatorSettings.GenerateCollectionsCount;
        public bool GenerateExtensions => _generatorSettings.GenerateExtensions;
        public int MaxGenerationDepth => _generatorSettings.MaxGenerationDepth;
        public int MaxCollectionSize => _generatorSettings.MaxCollectionSize;
        public int MaxDictionarySize => _generatorSettings.MaxDictionarySize;
        public int MaxPushDepth => _generatorSettings.MaxPushDepth;
        
        public static Dictionary<Type, Type> PrimitiveTypesMap = new()
        {
            { typeof(float), typeof(float) },
            
            { typeof(double), typeof(double) },
            { typeof(decimal), typeof(double) },
            { typeof(long), typeof(double) },
            { typeof(ulong), typeof(double) },
            
            { typeof(int), typeof(int) },
            { typeof(short), typeof(int) },
            { typeof(byte), typeof(int) },
            { typeof(ushort), typeof(int) },
            { typeof(sbyte), typeof(int) },
            
            { typeof(string), typeof(string) },
            { typeof(char), typeof(string) },
            
            { typeof(bool), typeof(bool) },
        };

        private WatchGenerationSchema _generationSchema;
        private GeneratorSettings _generatorSettings;
        private HashSet<Type> _totalAddedTypes = new();
        private Dictionary<Type, TypeGenerationInfo> _typeInfos = new();

        public void GenerateFromDescriptorsToPath(
            string outputFilePath, 
            string className, 
            string classNamespace, 
            ClassSettings classSettings, 
            GeneratorSettings generatorSettings,
            WatchGenerationSchema schema)
        {
            _generatorSettings = generatorSettings;
            _generationSchema = schema;
            
            var text = GenerateFromDescriptors(className, classNamespace, classSettings);
            SaveTextToFile(text, outputFilePath);
        }

        private string GenerateFromDescriptors(
            string className, 
            string classNamespace, 
            ClassSettings classSettings)
        {
            var classText = new StringBuilder();
            var indent = string.Empty;
            var hasNamespace = !string.IsNullOrWhiteSpace(classNamespace);
            
            _typeInfos.Clear();
            _totalAddedTypes.Clear();
            
            classText.AppendLine("#pragma warning disable CS0162");
            classText.AppendLine("using System;");
            classText.AppendLine("using System.Linq;");
            classText.AppendLine("using System.Collections.Generic;");
            classText.AppendLine("using Ingvar.LiveWatch;");
            classText.AppendLine();
            
            if (hasNamespace)
            {
                classText.AppendLine($"namespace {classNamespace}");
                classText.AppendLine("{");

                indent += "\t";
            }

            classText.AppendLine(indent + "// It's completely generated class, avoid modifying!");
            classText.AppendLine(indent + $"{classSettings.ModifierStr} {classSettings.StaticStr} {classSettings.PartialStr} class {className}");
            classText.AppendLine(indent + "{");
            indent += "\t";

            if (_generationSchema != null)
            {
                var generationWatches = Schema.GenerationWatches(_generationSchema);

                if (generationWatches.Count > 0)
                {
                    classText.AppendLine(indent + "private static string _tempStr;");
                    classText.AppendLine(indent + "private static HashSet<string> _tempStrSet = new();");
                    classText.AppendLine(indent + "private static Dictionary<object, HashSet<string>> _tempSetDict = new();");

                    foreach (var watchType in generationWatches.Keys)
                    {
                        var descriptor = WatchEditorServices.GenerationDescriptorMerger.GetMergedDescriptorFromSchema(
                            watchType, _generationSchema);
                        
                        BuildTypeInfosRecursive(descriptor, 1);
                    }

                    foreach (var typeInfoPair in _typeInfos)
                        typeInfoPair.Value.Childs.RemoveAll(c => !IsPrimitiveType(c.Type, out _) && !_typeInfos.ContainsKey(c.Type));
                    
                    foreach (var typeInfoPair in _typeInfos)
                        GenerateForTypeRecursive(typeInfoPair.Value.Descriptor, classText, indent);
                }
            }

            classText.AppendLine((hasNamespace ? "\t" : string.Empty) + "}");
            
            if (hasNamespace)
            {
                classText.AppendLine("}");
            }

            Debug.Log($"Watch generation succeed! Class name: {className}. Total types added: {_totalAddedTypes.Count}");
            
            return classText.ToString();
        }

        private void BuildTypeInfosRecursive(WatchVariableDescriptor descriptor, int recursionDepth)
        {
            var targetType = Descriptor.TargetType(descriptor);
            
            if (_typeInfos.ContainsKey(targetType))
                return;
            
            if (recursionDepth > MaxGenerationDepth)
            {
                Debug.LogWarning($"Skipped generation for type and its members because reached max recursion depth. Type: {targetType.FullName}");
                return;
            }

            var typeInfo = new TypeGenerationInfo()
            {
                Descriptor = descriptor
            };

            var typesToGenerate = new HashSet<Type>();
            typeInfo.Childs = GetChildren(descriptor, typesToGenerate);
            _typeInfos.Add(targetType, typeInfo);

            foreach (var typeToGenerate in typesToGenerate)
            {
                if (_typeInfos.ContainsKey(typeToGenerate))
                    continue;
                
                var childDescriptor = WatchEditorServices.GenerationDescriptorMerger.GetMergedDescriptorFromSchema(
                    typeToGenerate, _generationSchema);

                BuildTypeInfosRecursive(childDescriptor, recursionDepth + 1);
            }
        }

        private void GenerateForTypeRecursive(
            WatchVariableDescriptor descriptor, 
            StringBuilder classText, 
            string indent)
        {
            var targetType = Descriptor.TargetType(descriptor);

            if (!_typeInfos.TryGetValue(targetType, out var info)
                || _totalAddedTypes.Contains(targetType))
                return;
            
            _totalAddedTypes.Add(targetType);
            GenerateForType(descriptor, classText, indent, info.Childs);

            foreach (var child in info.Childs)
            {
                if (!_typeInfos.TryGetValue(child.Type, out var childInfo)
                    || IsPrimitiveType(targetType, out _))
                    continue;

                GenerateForTypeRecursive(
                    childInfo.Descriptor, 
                    classText, 
                    indent);
            }
        }

        private void GenerateForType(
            WatchVariableDescriptor descriptor, 
            StringBuilder classText, 
            string indent, 
            List<ChildInfo> childInfos)
        {
            var isDictionary = IsDictionary(Descriptor.TargetType(descriptor));
            var isCollection = IsCollection(Descriptor.TargetType(descriptor)) && Descriptor.TargetType(descriptor) != typeof(string);
            
            classText.AppendLine();
            classText.AppendLine($"\t\t#region {GetFullNameForType(Descriptor.TargetType(descriptor))}");

            GenerateGetOrAddWatchMethod(Descriptor.TargetType(descriptor), classText, indent);
            GenerateGetOrAddWatchChildMethod(Descriptor.TargetType(descriptor), classText, indent);
            
            if (isDictionary)
                GenerateSetupMethodForDictionary(Descriptor.TargetType(descriptor), classText, indent, childInfos);
            else if (isCollection)
                GenerateSetupMethodForCollection(Descriptor.TargetType(descriptor), classText, indent, childInfos);
            else
                GenerateSetupMethod(Descriptor.TargetType(descriptor), classText, indent, childInfos);

            if (isDictionary)
            {
                GeneratePushMethodForDictionary(Descriptor.TargetType(descriptor), classText, indent, childInfos);
            }
            else if (isCollection)
            {
                GeneratePushMethodForCollection(Descriptor.TargetType(descriptor), classText, indent, childInfos);
            }
            else
            {
                GeneratePushMethod(Descriptor.TargetType(descriptor), classText, indent, childInfos);
            }

            GenerateDirectPushWatchMethod(Descriptor.TargetType(descriptor), classText, indent);

            if (GenerateExtensions)
            {
                GeneratePushExtensionMethod(Descriptor.TargetType(descriptor), classText, indent);
                GenerateAddChildExtensionMethod(Descriptor.TargetType(descriptor), classText, indent);
                GenerateSetupExtensionMethod(Descriptor.TargetType(descriptor), classText, indent);
            }

            classText.AppendLine();
            classText.AppendLine($"\t\t#endregion");
        }

        private void GenerateGetOrAddWatchMethod(Type type, StringBuilder classText, string indent)
        {
            classText.AppendLine();
            classText.AppendLine(indent + $"public static WatchReference<{GetFullNameForType(type)}> GetOrAdd(string path, Func<{GetFullNameForType(type)}> valueGetter)");
            classText.AppendLine(indent + "{");
            DefineIf(classText, ScriptingDefineWithBuild);
            classText.AppendLine(indent + "\t" + $"var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<{GetFullNameForType(type)}>(path));");
            classText.AppendLine();
            GenerateUpdateCallSetup(type, classText, indent);
            classText.AppendLine(indent + "\t" + $"return watchReference;");
            DefineElse(classText);
            classText.AppendLine(indent + "\t" + $"return WatchServices.ReferenceCreator.Empty<{GetFullNameForType(type)}>();");
            DefineEndIf(classText);
            classText.AppendLine(indent + "}");
        }

        private void GenerateGetOrAddWatchChildMethod(Type type, StringBuilder classText, string indent)
        {
            classText.AppendLine();
            classText.AppendLine(indent + $"public static WatchReference<{GetFullNameForType(type)}> GetOrAdd<T>(WatchReference<T> parent, string path, Func<{GetFullNameForType(type)}> valueGetter)");
            classText.AppendLine(indent + "{");
            DefineIf(classText, ScriptingDefineWithBuild);
            classText.AppendLine(indent + "\t" + $"var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<{GetFullNameForType(type)}, T>(parent, path));");
            classText.AppendLine();
            GenerateUpdateCallSetup(type, classText, indent);
            classText.AppendLine(indent + "\t" + $"return watchReference;");
            DefineElse(classText);
            classText.AppendLine(indent + "\t" + $"return WatchServices.ReferenceCreator.Empty<{GetFullNameForType(type)}>();");
            DefineEndIf(classText);
            classText.AppendLine(indent + "}");
        }

        private void GenerateDirectPushWatchMethod(Type type, StringBuilder classText, string indent)
        {
            classText.AppendLine();
            classText.AppendLine(indent + $"public static WatchReference<{GetFullNameForType(type)}> Push(string path, {GetFullNameForType(type)} value, int maxRecursionDepth = {MaxPushDepth})");
            classText.AppendLine(indent + "{");
            classText.AppendLine(indent + "\t" + $"return Push(WatchServices.ReferenceCreator.GetOrAdd<{GetFullNameForType(type)}>(path), value, maxRecursionDepth);");
            classText.AppendLine(indent + "}");
        }

        private void GenerateAddChildExtensionMethod(Type type, StringBuilder classText, string indent)
        {
            classText.AppendLine();
            classText.AppendLine(indent + $"internal static WatchReference<{GetFullNameForType(type)}> GetOrAddChild<T>(this WatchReference<T> parent, string path, Func<{GetFullNameForType(type)}> valueGetter)");
            classText.AppendLine(indent + "{");
            classText.AppendLine(indent + "\t" + $"return GetOrAdd(parent, path, valueGetter);");
            classText.AppendLine(indent + "}");
        }
        
        private void GeneratePushExtensionMethod(Type type, StringBuilder classText, string indent)
        {
            classText.AppendLine();
            classText.AppendLine(indent + $"internal static WatchReference<{GetFullNameForType(type)}> PushValue(this WatchReference<{GetFullNameForType(type)}> watchReference, {GetFullNameForType(type)} value, int maxRecursionDepth = {MaxPushDepth})");
            classText.AppendLine(indent + "{");
            classText.AppendLine(indent + "\t" + $"return Push(watchReference, value, maxRecursionDepth);");
            classText.AppendLine(indent + "}");
        }

        private void GenerateSetupExtensionMethod(Type type, StringBuilder classText, string indent)
        {
            classText.AppendLine();
            classText.AppendLine(indent + $"internal static WatchReference<{GetFullNameForType(type)}> SetupWatch(this WatchReference<{GetFullNameForType(type)}> watchReference)");
            classText.AppendLine(indent + "{");
            classText.AppendLine(indent + "\t" + $"return Setup(watchReference);");
            classText.AppendLine(indent + "}");
        }

        private void GeneratesSetDecimalFormatMethod(Type type, StringBuilder classText, string indent)
        {
            classText.AppendLine();
            classText.AppendLine(indent + $"public static WatchReference<{GetFullNameForType(type)}> SetDecimalFormat(this WatchReference<{GetFullNameForType(type)}> watchReference, int decimalPlaces)");
            classText.AppendLine(indent + "{");
            DefineIf(classText, ScriptingDefineWithBuild);
            classText.AppendLine(indent + "\t" + "watchReference.WatchVariable.Values.DecimalValueFormat.PlacesAfterDot = (byte)decimalPlaces;");
            classText.AppendLine();
            DefineEndIf(classText);
            classText.AppendLine(indent + "\t" + "return watchReference;");
            classText.AppendLine(indent + "}");
        }

        private void DefineIf(StringBuilder classText, string define)
        {
            classText.AppendLine($"#if {define}");
        }
        
        private void DefineElse(StringBuilder classText)
        {
            classText.AppendLine($"#else");
        }
        
        private void DefineEndIf(StringBuilder classText)
        {
            classText.AppendLine($"#endif");
        }

        private void SaveTextToFile(string data, string filePath)
        {
            File.WriteAllText(filePath, data);
        }

        class TypeGenerationInfo
        {
            public WatchVariableDescriptor Descriptor;
            public List<ChildInfo> Childs = new ();
        }
    }
}