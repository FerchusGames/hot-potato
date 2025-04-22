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
        private void GenerateReturnChecks(Type type, StringBuilder classText, string indent)
        {
            classText.AppendLine(indent + "\t" + "if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))");
            classText.AppendLine(indent + "\t\t" + "return watchReference;");
            classText.AppendLine();
        }
        
        private void GenerateSetup(Type type, StringBuilder classText, string indent, List<ChildInfo> childInfos)
        {
            if (!DontGenerateChildCode && childInfos.Any(c => IsGeneratedType(c.Type) && (c.IsSortOrderSet || c.IsAlwaysCollapsable || c.IsDecimalPlacesSet || c.IsTraceable || c.IsMinMaxModeSet)))
            {
                classText.AppendLine(indent + "\t" + $"if (!WatchServices.ReferenceCreator.IsSetUp(watchReference))");
                classText.AppendLine(indent + "\t" + "{");
                
                foreach (var child in childInfos)
                {
                    if (!IsGeneratedType(child.Type))
                        continue;
                    
                    var extraSetupStr = string.Empty;

                    if (child.IsSortOrderSet)
                        extraSetupStr += $".SetSortOrder({child.SortOrder})";
                       
                    if (child.IsDecimalPlacesSet)
                        extraSetupStr += $".SetDecimalPlaces({child.DecimalPlaces})";
                    
                    if (child.IsAlwaysCollapsable)
                        extraSetupStr += $".SetAlwaysCollapsable()";
                    
                    if (child.IsTraceable)
                        extraSetupStr += $".SetTraceable()";
                    
                    if (child.IsMinMaxModeSet && child.MinMax.Mode == WatchMinMaxMode.Global)
                        extraSetupStr += $".SetMinMaxModeAsGlobal()";
                    
                    if (child.IsMinMaxModeSet && child.MinMax.Mode == WatchMinMaxMode.Custom)
                        extraSetupStr += $".SetMinMaxModeAsCustom({child.MinMax.CustomMinValue},{child.MinMax.CustomMaxValue})";
                    
                    if (string.IsNullOrWhiteSpace(extraSetupStr))
                        continue;
                    
                    var childName = GetChildNameString(type, child);
                    classText.AppendLine(indent + "\t\t" + $"WatchServices.ReferenceCreator.GetOrAdd<{child.TypeString}, {GetFullNameForType(type)}>(watchReference, {childName}){extraSetupStr};");
                }
                
                classText.AppendLine(indent + "\t" + "}");
            }
            
            var valueTypeName = PrimitiveNamesMap[typeof(string)];

            if (IsPrimitiveType(type, out var storeType))
                valueTypeName = PrimitiveNamesMap[storeType];
            
            classText.AppendLine(indent + "\t" + $"return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.{valueTypeName});");
        }
        
        private void GenerateSetupCall(Type type, StringBuilder classText, string indent)
        {
            classText.AppendLine(indent + "\t" + "Setup(watchReference);");
        }
        
        private void GenerateNullCheck(Type type, StringBuilder classText, string indent)
        {
            if (type.IsValueType)
                return;

            classText.AppendLine(indent + "\t" + "if (value == null)");
            classText.AppendLine(indent + "\t\t" + "return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);");
            classText.AppendLine();
        }

        private void GenerateUpdateCallSetup(Type type, StringBuilder classText, string indent)
        {
            classText.AppendLine(indent + "\t" + $"WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>");
            classText.AppendLine(indent + "\t" + "{");
            classText.AppendLine(indent + "\t\t" + "var value = valueGetter();");
            classText.AppendLine(indent + "\t\t" + "Push(watchReference, value);");
            classText.AppendLine(indent + "\t" + "});");
            classText.AppendLine();
        }
        
        private void GenerateChildsSetup(Type type, StringBuilder classText, string indent, List<ChildInfo> childInfos)
        {
            //Removed in case of infinite loop like gameobject -> transofform -> gameobject. Needed recursionDepth otherwise
            
            if (childInfos.Count <= 0 || DontGenerateChildCode)
                return;

            foreach (var child in childInfos)
            {
                var name = child.AccessModifier == MemberAccessModifier.Public && child.MemberName == child.ShownName ? $"nameof({GetFullNameForType(type)}.{child.MemberName})" : $"\"{child.ShownName}\"";
                var setupMethod = IsPrimitiveType(child.Type, out _) ? "Watch.Setup" : "Setup";
                classText.AppendLine(indent + "\t" + $"{setupMethod}(WatchServices.ReferenceCreator.GetOrAdd<{child.TypeString}, {GetFullNameForType(type)}>(watchReference, {name}));");
            }

            classText.AppendLine();
        }

        private void GenerateChildsPush(Type type, StringBuilder classText, string indent, List<ChildInfo> childInfos)
        {
            if (childInfos.Count <= 0 || DontGenerateChildCode)
                return;

            foreach (var child in childInfos)
            {
                if (!IsGeneratedType(child.Type))
                    continue;
                
                var getter = $"value.{child.MemberName}" + (child.IsMethod ? "()" : string.Empty);
                var name = GetChildNameString(type, child);
                var pushMethod = IsPrimitiveType(child.Type, out _) ? "Watch.Push" : "Push";
                classText.AppendLine(indent + "\t" + $"{pushMethod}(WatchServices.ReferenceCreator.GetOrAdd<{child.TypeString}, {GetFullNameForType(type)}>(watchReference, {name}), {getter}, maxRecursionDepth - 1);");
            }

            classText.AppendLine();
        }

        private string GetChildNameString(Type type, ChildInfo child)
        {
            var name = child.AccessModifier == MemberAccessModifier.Public && child.MemberName == child.ShownName 
                ? $"nameof({GetFullNameForType(type)}.{child.MemberName})" 
                : $"\"{child.ShownName}\"";
            if (type.IsArray && child.MemberName == "Length")
                name = "nameof(Array.Length)";

            return name;
        }
        
        private void PushNonBasicValue(Type type, StringBuilder classText, string indent)
        {
            classText.AppendLine(indent + "\t" + "WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);");
        }
    }
}