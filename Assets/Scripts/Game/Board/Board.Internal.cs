using UnityEngine;

namespace Konane.Game
{
    public partial class Board
    {
        void DoSetName(string name)
        {
            gameObject.name = name;
        }

        void DoSetState(BoardState state)
        {
            switch (state)
            {
                case BoardState.None:
                    SetAsNone();
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

        void ToggleOccupiable(bool active)
        {
            m_OccupiableIcon.enabled = active;
        }

        void OnPieceDespawn(Piece piece)
        {
            if (Piece == null)
                return;

            Piece.OnDespawn.RemoveListener(OnPieceDespawn);
            Piece = null;
        }

        protected override void OnButtonDown()
        {
            OnDown?.Invoke(this);
        }
    }
}