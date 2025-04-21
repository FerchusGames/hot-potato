using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    public static class AssetExtensions
    {
        public static IEnumerable<T> FindAssetsByType<T>() where T : Object 
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
            
            foreach (var guid in guids) 
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                
                if (asset != null) 
                {
                    yield return asset;
                }
            }
        }

        public static void SaveAndRefresh(Object asset)
        {
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssetIfDirty(asset);
            AssetDatabase.Refresh();
        }
    }
}