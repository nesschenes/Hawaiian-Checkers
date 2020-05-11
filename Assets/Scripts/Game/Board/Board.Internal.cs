using UnityEngine;

namespace Konane.Game
{
    public partial class Board
    {
        void Awake()
        {
            m_Button.OnDown.AddListener(OnButtonDown);
        }

        void OnDestroy()
        {
            m_Button.OnDown.RemoveAllListeners();
        }

        void DoSetState(BoardState state)
        {
            SetNoneState();

            switch (state)
            {
                case BoardState.None:
                    break;
                case BoardState.Occupiable:
                    ToggleOccupiable(true);
                    break;
            }
        }

        void DoSetCoordinate(Coordinate coordinate)
        {
            transform.position = GameUtility.CoordinateToPosition(coordinate);
        }

        void DoSetColor(Color color)
        {
            m_Icon.color = color;
        }

        void SetRandomUV()
        {
            var x = Random.Range(0, 256);
            var y = Random.Range(0, 256);
            m_Icon.material.SetInt("_Row", x);
            m_Icon.material.SetInt("_Col", y);
        }

        void SetNoneState()
        {
            SetInteractable(false);
            ToggleOccupiable(false);
        }

        void ToggleOccupiable(bool active)
        {
            SetInteractable(true);
            m_OccupiableIcon.enabled = active;
        }

        void OnPieceDespawn(Piece piece)
        {
            if (Piece == null)
                return;

            Piece.gameObject.SetActive(false);
            Piece.OnDespawn.RemoveListener(OnPieceDespawn);
            Piece = null;
        }

        void OnButtonDown()
        {
            OnDown.Invoke(this);
        }
    }
}