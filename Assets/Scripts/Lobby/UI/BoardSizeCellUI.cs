using System;
using UnityEngine;
using UnityEngine.UI;

namespace Konane.Lobby.UI
{
    public class BoardSizeCellUI : MonoBehaviour
    {
        [SerializeField]
        Button m_Button = null;
        [SerializeField]
        Text m_Content = null;

        int mBoardSize = 6;
        Action<int> mOnSelected = delegate { };

        void Awake()
        {
            m_Button.onClick.AddListener(OnSelected);
        }

        void OnDestroy()
        {
            m_Button.onClick.RemoveListener(OnSelected);
        }

        public void Show(int size, Action<int> onSelected)
        {
            gameObject.SetActive(true);
            mBoardSize = size;
            mOnSelected = onSelected;
            m_Content.text = $"{size} X {size}";
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        void OnSelected()
        {
            mOnSelected.Invoke(mBoardSize);
        }
    }
}