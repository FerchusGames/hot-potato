using UnityEditor;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    public class MenuWatchScriptTemplateCreator
    {
        [MenuItem("Assets/Create/LiveWatch/WatchManagerScript", false)]
        public static void CreateWatchTemplates()
        {
            ProjectWindowUtil.CreateAssetWithContent("WatchManager.cs", 
                @"using Ingvar.LiveWatch;
using UnityEngine;

// Don't forget to attach this script to some object on your scene
public class WatchManager : MonoBehaviour
{
    private static WatchManager _instance;
    
    private void Awake()
    {
        // Single instance check + preserve between scenes. Remove if not needed
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }

        // All new watches must be added only AFTER this call
        // You can remove this call if you want to preserve watch data from the previous session
        Watch.DestroyAll();
    }

    private void Start()
    {
        // You can declare new watches here or anywhere else in project
        
        // Auto watch example (SetAlwaysCollapsable() is needed to prevent spamming)
        Watch.GetOrAdd(""Frame"", () => Time.frameCount).SetAlwaysCollapsable();
        
        // Manual watch example
        Watch.Push(""Log"", ""Hello World!"");
    }

    private void LateUpdate()
    {
        // All manual watch Push() methods in project must be called BEFORE this call
        // Updates ALL auto watches in project. Avoid calling more than once per update
        Watch.UpdateAll();
    }
}"
                , EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D);
        }
    }
}