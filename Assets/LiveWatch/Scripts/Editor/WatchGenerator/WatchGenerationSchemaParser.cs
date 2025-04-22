using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Ingvar.LiveWatch.Generation
{
    public class WatchGenerationSchemaParser
    {
        public WatchGenerationSchema ReadSchemaFromPath(string schemaFullPath)
        {
            var fileDataString = ReadFile(schemaFullPath);
            return ReadSchema(fileDataString);
        }

        public WatchGenerationSchema ReadSchema(string fileDataString)
        {
            if (string.IsNullOrWhiteSpace(fileDataString))
            {
                return null;
            }

            var classNames = GetClassNames(fileDataString);

            if (classNames == null || !classNames.Any())
            {
                return null;
            }

            var namespaceNames = GetClassNamespaces(fileDataString);

            var className = classNames.First();
            var namespaceName = namespaceNames == null || !namespaceNames.Any() ? string.Empty : namespaceNames.First();

            var fullName = string.IsNullOrWhiteSpace(namespaceName) ? className : $"{namespaceName}.{className}";

            var schemaType = GetTypeByFullName(fullName);

            if (schemaType == null)
            {
                return null;
            }

            var schemaInstance = CreateInstanceOfType(schemaType);
            
            if (schemaInstance == null)
            {
                return null;
            }

            var schema = (WatchGenerationSchema) schemaInstance;
            
            schema.OnDefine();
            schema.OnGenerate();
            
            return schema;
        }

        private static string ReadFile(string filePath)
        {
            try
            {
                var fileData = File.ReadAllText(filePath);
                return fileData;
            }
            catch
            {
                Debug.LogError($"Failed to read schema file. Path: {filePath}");
                throw;
            }
        }

        private static List<string> GetClassNames(string classStringRaw)
        {
            var pattern = @"(?<=class)(.*)(?=:." + nameof(WatchGenerationSchema) + ")";
            var matches = Regex.Matches(classStringRaw, pattern, RegexOptions.Multiline);

            if (matches.Count == 0)
            {
                Debug.LogError($"No classes in schema file");
                return new List<string>();
            }
            
            var classNames = matches
                .Cast<Match>()
                .Select(x => x.Value.Trim())
                .ToList();
            
            return classNames;
        }

        private static List<string> GetClassNamespaces(string classStringRaw)
        {
            var pattern = @"(?<=namespace)(.*)(?=\s.*{)";
            var matches = Regex.Matches(classStringRaw, pattern, RegexOptions.Multiline);

            if (matches.Count == 0)
            {
                return new List<string>();
            }
            
            var namespaceNames = matches
                .Cast<Match>()
                .Select(x => x.Value.Trim())
                .ToList();
            
            return namespaceNames;
        }
        
        private static Type GetTypeByFullName(string typeFullName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var type = assembly.GetType(typeFullName, false);

                if (type != null)
                    return type;
            }

            Debug.Log($"Failed to find type with name: {typeFullName}");
            
            return null;
        }
        
        private static object CreateInstanceOfType(Type type)
        {
            try
            {
                var obj = Activator.CreateInstance(type);
                return obj;
            }
            catch
            {
                Debug.LogError($"Failed to create instance of {type.Name}");
                throw;
            }
        }
    }
}