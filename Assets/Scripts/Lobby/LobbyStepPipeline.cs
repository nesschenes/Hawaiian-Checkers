using Konane.Game;
using Konane.Utility;
using UnityEngine;

namespace Konane.Lobby
{
    public class LobbyStepPipeline : MonoBehaviour
    {
        void Awake()
        {
            Notify.LoadGame += OnLoadGame;

            CheckResumableGame();
        }

        void OnDestroy()
        {
            Notify.LoadGame -= OnLoadGame;
        }

        void CheckResumableGame()
        {
            var data = GameUtility.Load();
            if (data != null)
                Dialog.Instance.Show("Resume last game ?", () => OnConfirmResume(data), OnCancelResume);
        }

        void OnLoadGame()
        {
            SceneUtility.LoadGameScene();
            SceneUtility.LoadGameUIScene();
        }

        void OnConfirmResume(GameData data)
        {
            GameSettings.PieceTypeToBegin = data.CurrentPieceType;
            GameSettings.PieceTypeCount = data.PieceTypeCount;
            GameSettings.BoardRowsCount = data.BoardRowsCount;
            GameData.LastData = data;
            OnLoadGame();
        }

        void OnCancelResume()
        {
            GameUtility.Clear();
        }
    }
}