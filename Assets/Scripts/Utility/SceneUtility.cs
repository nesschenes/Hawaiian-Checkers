using UnityEngine.SceneManagement;

namespace Konane.Utility
{
    public static class SceneUtility
    {
        public static void LoadMenuScene()
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }

        public static void LoadMenuUIScene()
        {
            SceneManager.LoadScene("MenuUI", LoadSceneMode.Additive);
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