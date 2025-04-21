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
        public static Dictionary<Type, string> PrimitiveNamesMap = new()
        {
            { typeof(float), "Float" },
            { typeof(double), "Double" },
            { typeof(int), "Int" },
            { typeof(string), "String" },
            { typeof(bool), "Bool" },
            { typeof(decimal), "Decimal" },
            { typeof(long), "Long" },
            { typeof(short), "Short" },
            { typeof(byte), "Byte" },
            { typeof(ulong), "ULong" },
            { typeof(ushort), "UShort" },
            { typeof(sbyte), "SByte" },
            { typeof(char), "Char" },
        };

        private void GeneratePushMethod(Type type, StringBuilder classText, string indent, List<ChildInfo> childInfos)
        {
            classText.AppendLine();
            classText.AppendLine(indent + $"public static WatchReference<{GetFullNameForType(type)}> Push(WatchReference<{GetFullNameForType(type)}> watchReference, {GetFullNameForType(type)} value, int maxRecursionDepth = {MaxPushDepth})");
            classText.AppendLine(indent + "{");

            DefineIf(classText, ScriptingDefineWithBuild);
            GenerateReturnChecks(type, classText, indent);
            GenerateSetupCall(type, classText, indent);
            GenerateNullCheck(type, classText, indent);

            if (IsPrimitiveType(type, out _))
            {
                var typeName = PrimitiveNamesMap[type];
                classText.AppendLine(indent + "\t" + $"WatchServices.ReferenceCreator.Push{typeName}(watchReference, value);");
            }
            else if (type.IsEnum)
            {
                classText.AppendLine(indent + "\t" + $"WatchServices.ReferenceCreator.Push{PrimitiveNamesMap[typeof(string)]}(watchReference, WatchServices.NameBuilder.GetStringFromEnum<{GetFullNameForType(type)}>((int)value));");
            }
            else
            {
                GenerateChildsPush(type, classText, indent, childInfos);
                PushNonBasicValue(type, classText, indent);
            }
            DefineEndIf(classText);
            classText.AppendLine(indent + "\t" + "return watchReference;");
            classText.AppendLine(indent + "}");
        }

        private void GenerateSetupMethod(Type type, StringBuilder classText, string indent, List<ChildInfo> childInfos)
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
    }
}