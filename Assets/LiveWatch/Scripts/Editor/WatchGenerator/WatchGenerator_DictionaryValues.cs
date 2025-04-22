using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Ingvar.LiveWatch.Generation
{
    public partial class WatchGenerator
    {
        private void GeneratePushMethodForDictionary(Type type, StringBuilder classText, string indent, List<ChildInfo> childInfos)
        {
            var elementTypes = GetDictionaryElementType(type);
            var isSimpleKey = IsSimpleDictionaryKeyType(elementTypes.key);
            
            classText.AppendLine();
            classText.AppendLine(indent + $"public static WatchReference<{GetFullNameForType(type)}> Push(WatchReference<{GetFullNameForType(type)}> watchReference, {GetFullNameForType(type)} value, int maxRecursionDepth = {MaxPushDepth})");
            classText.AppendLine(indent + "{");
            
            DefineIf(classText, ScriptingDefineWithBuild);
            GenerateReturnChecks(type, classText, indent);
            GenerateSetupCall(type, classText, indent);
            GenerateNullCheck(type, classText, indent);
            GenerateChildsPush(type, classText, indent, childInfos);

            if (elementTypes.key != null && IsGeneratedType(elementTypes.key)
                && elementTypes.value != null && IsGeneratedType(elementTypes.value)
                && !DontGenerateChildCode)
            {
                var keyPushMethodStr = IsPrimitiveType(elementTypes.key, out _) ? "Watch.Push" : "Push";
                var valuePushMethodStr = IsPrimitiveType(elementTypes.value, out _) ? "Watch.Push" : "Push";

                classText.AppendLine(indent + "\t" + "var counter = 0;");
                classText.AppendLine(indent + "\t" + "if (!_tempSetDict.TryGetValue(value, out var strSet))");
                classText.AppendLine(indent + "\t\t" + "_tempSetDict.Add(value, strSet = new HashSet<string>());");
                classText.AppendLine(indent + "\t" + "else");
                classText.AppendLine(indent + "\t\t" + "strSet.Clear();");
                classText.AppendLine();

                classText.AppendLine(indent + "\t" + "foreach (var pair in value)");
                classText.AppendLine(indent + "\t" + "{");
                if (isSimpleKey)
                {
                    classText.AppendLine(indent + "\t\t" + $"var str = {GetDictionaryItemName(elementTypes.key)};");
                    classText.AppendLine(indent + "\t\t" + "strSet.Add(str);");
                    classText.AppendLine();
                    classText.AppendLine(indent + "\t\t" + $"var valueWatch = {valuePushMethodStr}(WatchServices.ReferenceCreator.GetOrAdd<{GetFullNameForType(elementTypes.value)},{GetFullNameForType(type)}>(watchReference, str), pair.Value, maxRecursionDepth - 1);");
                    classText.AppendLine(indent + "\t\t" + $"WatchServices.ReferenceCreator.MarkAsDictionaryValue(valueWatch);");
                }
                else
                {
                    classText.AppendLine(indent + "\t\t" + "var hashCode = pair.Key.GetHashCode();");
                    classText.AppendLine();
                    classText.AppendLine(indent + "\t\t" + "var str = WatchServices.NameBuilder.GetDictionaryKeyName(hashCode);");
                    classText.AppendLine(indent + "\t\t" + $"var keyWatch = {keyPushMethodStr}(WatchServices.ReferenceCreator.GetOrAdd<{GetFullNameForType(elementTypes.key)},{GetFullNameForType(type)}>(watchReference, str), pair.Key, maxRecursionDepth - 1);");
                    classText.AppendLine(indent + "\t\t" + "strSet.Add(str);");
                    classText.AppendLine(indent + "\t\t" + $"WatchServices.ReferenceCreator.MarkAsDictionaryValue(keyWatch);");
                    classText.AppendLine();
                    classText.AppendLine(indent + "\t\t" + "str = WatchServices.NameBuilder.GetDictionaryValueName(hashCode);");
                    classText.AppendLine(indent + "\t\t" + $"var valueWatch = {valuePushMethodStr}(WatchServices.ReferenceCreator.GetOrAdd<{GetFullNameForType(elementTypes.value)},{GetFullNameForType(type)}>(watchReference, str), pair.Value, maxRecursionDepth - 1);");
                    classText.AppendLine(indent + "\t\t" + "strSet.Add(str);");
                    classText.AppendLine(indent + "\t\t" + $"WatchServices.ReferenceCreator.MarkAsDictionaryValue(valueWatch);");
                }
                classText.AppendLine();
                classText.AppendLine(indent + "\t\t" + $"if (++counter >= {MaxDictionarySize})");
                classText.AppendLine(indent + "\t\t\t" + "break;");

                classText.AppendLine(indent + "\t" + "}");
                classText.AppendLine();
                classText.AppendLine(indent + "\t" + "foreach (var childName in watchReference.GetChildNames())");
                classText.AppendLine(indent + "\t" + "{");
                if (isSimpleKey)
                {
                    classText.AppendLine(indent + "\t\t" + $"if (!strSet.Contains(childName) && WatchServices.ReferenceCreator.IsDictionaryValue(WatchServices.ReferenceCreator.GetOrAdd<Any, {GetFullNameForType(type)}>(watchReference, childName)))");
                    classText.AppendLine(indent + "\t\t\t" + $"WatchServices.ReferenceCreator.PushEmpty(WatchServices.ReferenceCreator.GetOrAdd<{GetFullNameForType(elementTypes.value)},{GetFullNameForType(type)}>(watchReference, childName), true, maxRecursionDepth - 1);");
                }
                else
                {
                    classText.AppendLine(indent + "\t\t" + "if (strSet.Contains(childName))");
                    classText.AppendLine(indent + "\t\t\t" + "continue;");
                    classText.AppendLine();
                    classText.AppendLine(indent + "\t\t" + $"var genericTypeChild = WatchServices.ReferenceCreator.GetOrAdd<Any, {GetFullNameForType(type)}>(watchReference, childName);");
                    classText.AppendLine();
                    classText.AppendLine(indent + "\t\t" + "if (WatchServices.ReferenceCreator.IsDictionaryKey(genericTypeChild))");
                    classText.AppendLine(indent + "\t\t\t" + $"WatchServices.ReferenceCreator.PushEmpty(WatchServices.ReferenceCreator.GetOrAdd<{GetFullNameForType(elementTypes.key)},{GetFullNameForType(type)}>(watchReference, childName), true, maxRecursionDepth - 1);");
                    classText.AppendLine(indent + "\t\t" + "else if (WatchServices.ReferenceCreator.IsDictionaryValue(genericTypeChild))");
                    classText.AppendLine(indent + "\t\t\t" + $"WatchServices.ReferenceCreator.PushEmpty(WatchServices.ReferenceCreator.GetOrAdd<{GetFullNameForType(elementTypes.value)},{GetFullNameForType(type)}>(watchReference, childName), true, maxRecursionDepth - 1);");
                }

                classText.AppendLine(indent + "\t" + "}");
            }

            PushNonBasicValue(type, classText, indent);
            DefineEndIf(classText);
            classText.AppendLine(indent + "\t" + "return watchReference;");
            classText.AppendLine(indent + "}");
        }
        
        private void GenerateSetupMethodForDictionary(Type type, StringBuilder classText, string indent, List<ChildInfo> childInfos)
        {
            classText.AppendLine();
            classText.AppendLine(indent + $"public static WatchReference<{GetFullNameForType(type)}> Setup(WatchReference<{GetFullNameForType(type)}> watchReference)");
            classText.AppendLine(indent + "{");
            
            DefineIf(classText, ScriptingDefineWithBuild);
            GenerateSetup(type, classText, indent, childInfos);
            DefineEndIf(classText);
            classText.AppendLine(indent + "\t" + "return watchReference;");
            classText.AppendLine(indent + "}");
        }
        
        private (Type key, Type value) GetDictionaryElementType(Type dictionaryType)
        {
            return (dictionaryType.GetGenericArguments()[0], dictionaryType.GetGenericArguments()[1]);
        }

        private bool IsSimpleDictionaryKeyType(Type targetType)
        {
            return targetType.IsEnum || 
                   PrimitiveTypesMap.TryGetValue(targetType, out var storeType)
                   && (storeType == typeof(int) || storeType == typeof(string));
        }

        private string GetDictionaryItemName(Type keyType)
        {
            string nameGetterStr;
            
            if (keyType == typeof(string))
                nameGetterStr = "pair.Key";
            else if (keyType == typeof(char))
                nameGetterStr = "(string)pair.Key";
            else if (keyType.IsEnum)
                nameGetterStr = $"WatchServices.NameBuilder.GetStringFromEnum<{GetFullNameForType(keyType)}>((int)pair.Key)";
            else 
                nameGetterStr = "WatchServices.NameBuilder.GetString(pair.Key)";

            return nameGetterStr;
        }
    }
}