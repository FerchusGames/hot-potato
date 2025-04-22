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
        private void GeneratePushMethodForCollection(Type type, StringBuilder classText, string indent, List<ChildInfo> childInfos)
        {
            var nameGetterStr = "WatchServices.NameBuilder.GetCollectionItemName(index)";
            var itemsCountStr = $"watchReference.ChildCount - {childInfos.Count}";
            var elementType = GetCollectionElementType(type);
            
            classText.AppendLine();
            classText.AppendLine(indent + $"public static WatchReference<{GetFullNameForType(type)}> Push(WatchReference<{GetFullNameForType(type)}> watchReference, {GetFullNameForType(type)} value, int maxRecursionDepth = {MaxPushDepth})");
            classText.AppendLine(indent + "{");
            
            DefineIf(classText, ScriptingDefineWithBuild);
            GenerateReturnChecks(type, classText, indent);
            GenerateSetupCall(type, classText, indent);
            GenerateNullCheck(type, classText, indent);
            GenerateChildsPush(type, classText, indent, childInfos);
            
            if (elementType != null && !DontGenerateChildCode && IsGeneratedType(elementType))
            {
                var pushMethodStr = IsPrimitiveType(elementType, out _) ? "Watch.Push" : "Push";
                
                classText.AppendLine(indent + "\t" + "var index = 0;");
                classText.AppendLine();
                classText.AppendLine(indent + "\t" + "foreach (var item in value)");
                classText.AppendLine(indent + "\t" + "{");
                classText.AppendLine(indent + "\t\t" + $"var elementWatch = {pushMethodStr}(WatchServices.ReferenceCreator.GetOrAdd<{GetFullNameForType(elementType)},{GetFullNameForType(type)}>(watchReference, {nameGetterStr}), item, maxRecursionDepth - 1);");
                classText.AppendLine(indent + "\t\t" + $"WatchServices.ReferenceCreator.MarkAsCollectionValue(elementWatch);");
                classText.AppendLine();
                classText.AppendLine(indent + "\t\t" + $"if (++index >= {MaxCollectionSize})");
                classText.AppendLine(indent + "\t\t\t" + $"break;");
                classText.AppendLine(indent + "\t" + "}");
                classText.AppendLine();
                classText.AppendLine(indent + "\t" + $"if ({itemsCountStr} > index)");
                classText.AppendLine(indent + "\t" + "{");
                classText.AppendLine(indent + "\t\t" + $"for (; index < {itemsCountStr}; index++)");
                classText.AppendLine(indent + "\t\t\t" + $"WatchServices.ReferenceCreator.PushEmpty(WatchServices.ReferenceCreator.GetOrAdd<{GetFullNameForType(elementType)},{GetFullNameForType(type)}>(watchReference, {nameGetterStr}), true, maxRecursionDepth - 1);");
                classText.AppendLine(indent + "\t" + "}");
            }
            PushNonBasicValue(type, classText, indent);
            DefineEndIf(classText);
            classText.AppendLine(indent + "\t" + "return watchReference;");
            classText.AppendLine(indent + "}");
        }
        
        private void GenerateSetupMethodForCollection(Type type, StringBuilder classText, string indent, List<ChildInfo> childInfos)
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

        private Type GetCollectionElementType(Type collectionType)
        {
            if (collectionType.IsArray)
                return collectionType.GetElementType();
            
            if (collectionType.IsGenericType)
                return collectionType.GetGenericArguments()[0];

            if (collectionType.BaseType == null)
                return null;

            return GetCollectionElementType(collectionType.BaseType);
        }
    }
}