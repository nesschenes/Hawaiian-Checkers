using Konane.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Konane.Lobby.UI
{
    public class LobbyMenuUI : MonoBehaviour
    {
        [SerializeField]
        Button m_TwoPlayer = null;
        [SerializeField]
        Button m_ExitGame = null;
        [SerializeField]
        BoardSizeUI m_BoardSizeUI = null;
        [SerializeField]
        int[] m_BoardSizes = null;

        void Awake()
        {
            m_TwoPlayer.onClick.AddListener(OnTwoPlayer);
            m_ExitGame.onClick.AddListener(OnExitGame);
        }

        void OnDestroy()
        {
            m_TwoPlayer.onClick.RemoveListener(OnTwoPlayer);
            m_ExitGame.onClick.RemoveListener(OnExitGame);
        }

        void OnTwoPlayer()
        {
            m_BoardSizeUI.Show(m_BoardSizes, OnBoardSizeSelected);
        }

        void OnExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        void OnBoardSizeSelected(int size)
        {
            GameSettings.BoardRowsCount = size;

            Notify.LoadGame();
        }
    }
}