using UnityEngine.SceneManagement;

namespace Konane.Utility
{
    public static class SceneUtility
    {
        public static void LoadLobbyScene()
        {
            SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        }

        public static void LoadLobbyUIScene()
        {
            SceneManager.LoadScene("LobbyUI", LoadSceneMode.Additive);
        }

        public static void LoadGameScene()
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }

        public static void LoadGameUIScene()
        {
            SceneManager.LoadScene("GameUI", LoadSceneMode.Additive);
        }
    }
}