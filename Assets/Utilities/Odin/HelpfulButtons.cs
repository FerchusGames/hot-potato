#if UNITY_EDITOR

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Utilities.Odin
{
    public class HelpfulButtons : OdinEditorWindow
    {
        [MenuItem("Tools/Helpful Buttons")]
        public static void OpenWindow()
        {
            GetWindow<HelpfulButtons>().Show();
        }
        
        [Button, BoxGroup("Lobby")]
        private void LocalhostLobby()
        {
            LoadScene("Assets/HotPotato/Scenes/Menus/Lobby/LocalhostLobby.unity");
        }
        
        [Button, BoxGroup("Lobby")]
        private void SteamLobby()
        {
            LoadScene("Assets/HotPotato/Scenes/Menus/Lobby/SteamLobby.unity");
        }
        
        [Button, BoxGroup("Game")]
        private void MainGame()
        {
            LoadScene("Assets/HotPotato/Scenes/MainGame/MainGame.unity");
        }
        
        private void LoadScene(string scenePath)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(scenePath);
            }
        }
    }
}

#endif