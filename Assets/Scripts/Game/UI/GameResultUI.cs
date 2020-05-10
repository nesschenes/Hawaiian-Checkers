using UnityEngine;
using UnityEngine.UI;

namespace Konane.Game.UI
{
    public class GameResultUI : MonoBehaviour
    {
        [SerializeField]
        Image m_WinnerIcon = null;
        [SerializeField]
        Image m_WinIcon = null;
        [SerializeField]
        Sprite m_BlackWinnerIcon = null;
        [SerializeField]
        Sprite m_WhiteWinnerIcon = null;
    }
}