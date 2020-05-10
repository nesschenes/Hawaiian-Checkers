using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Konane.Lobby.UI
{
    public class BoardSizeUI : MonoBehaviour
    {
        [SerializeField]
        Canvas m_Canvas = null;
        [SerializeField]
        GraphicRaycaster m_Raycaster = null;
        [SerializeField]
        BoardSizeCellUI m_CellUI = null;
        [SerializeField]
        RectTransform m_CellUIPool = null;

        List<BoardSizeCellUI> mCellUIs = new List<BoardSizeCellUI>();

        void Awake()
        {
            PrespawnCellUI(2);
        }

        public void Show(int[] sizes, Action<int> onBoardSizeSelected)
        {
            m_Canvas.enabled = true;
            m_Raycaster.enabled = true;

            PrespawnCellUI(sizes.Length - mCellUIs.Count);
            for (var i = 0; i < sizes.Length; i++)
            {
                var size = sizes[i];
                var cell = mCellUIs[i];
                cell.Show(size, onBoardSizeSelected);
            }
        }

        public void Hide()
        {
            m_Canvas.enabled = false;
            m_Raycaster.enabled = false;

            for (var i = 0; i < mCellUIs.Count; i++)
            {
                var cell = mCellUIs[i];
                cell.Hide();
            }
        }

        void PrespawnCellUI(int count)
        {
            for (var i = 0; i < count; i++)
                SpawnCellUI();
        }

        BoardSizeCellUI SpawnCellUI()
        {
            var cell = Instantiate(m_CellUI, m_CellUIPool);
            mCellUIs.Add(cell);

            return cell;
        }
    }
}