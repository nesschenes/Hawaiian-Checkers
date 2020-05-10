using UnityEngine;
using UnityEngine.UI;

namespace Konane.Game.UI
{
    public class GameResultUI : MonoBehaviour
    {
        [SerializeField]
        Canvas m_Canvas = null;
        [SerializeField]
        Image m_GameResult = null;
        [SerializeField]
        Button m_BackToLobby = null;
        [SerializeField]
        Sprite[] m_GameResultSprites = null;

        void Awake()
        {
            Notify.GameResult += OnGameResult;
            m_BackToLobby.onClick.AddListener(OnBackToLobby);
        }

        void OnDestroy()
        {
            Notify.GameResult -= OnGameResult;
            m_BackToLobby.onClick.RemoveListener(OnBackToLobby);
        }

        void OnGameResult(int winner)
        {
            m_GameResult.sprite = m_GameResultSprites[winner - 1];
            m_Canvas.enabled = true;
        }

        void OnBackToLobby()
        {
            Notify.LoadLobby();
        }
    }
}