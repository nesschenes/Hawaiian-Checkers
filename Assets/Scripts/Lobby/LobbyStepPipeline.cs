using Konane.Utility;
using UnityEngine;

namespace Konane.Lobby
{
    public class LobbyStepPipeline : MonoBehaviour
    {
        void Awake()
        {
            Notify.LoadGame += OnLoadGame;
        }

        void OnDestroy()
        {
            Notify.LoadGame -= OnLoadGame;
        }

        void OnLoadGame()
        {
            SceneUtility.LoadGameScene();
            SceneUtility.LoadGameUIScene();
        }
    }
}