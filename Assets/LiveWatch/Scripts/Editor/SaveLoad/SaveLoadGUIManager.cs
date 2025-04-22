using UnityEditor;

namespace Ingvar.LiveWatch
{
    public static class SaveLoadGUIManager
    {
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.update += OnEditorUpdate;
        }
        
        private static void OnEditorUpdate()
        {
            if (WatchServices.SaveLoader.IsSaving)
            {
                var cancelRequired = EditorUtility.DisplayCancelableProgressBar(
                    "Save",
                    "Saving watches to a binary file",
                    WatchServices.SaveLoader.Progress.Progress);

                if (cancelRequired)
                {
                    WatchServices.SaveLoader.CancelSave();
                    EditorUtility.ClearProgressBar();
                }
            }

            if (WatchServices.SaveLoader.IsLoading)
            {
                var cancelRequired = EditorUtility.DisplayCancelableProgressBar(
                    "Load",
                    "Loading watches from a binary file",
                    WatchServices.SaveLoader.Progress.Progress);

                if (cancelRequired)
                {
                    WatchServices.SaveLoader.CancelLoad();
                    EditorUtility.ClearProgressBar();
                }
            }

        }
    }
}