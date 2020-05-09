using UnityEngine;

namespace Hawaiian.Game
{
    public partial class Board
    {
        void Awake()
        {
            m_Button.OnDown.AddListener(OnButtonDown);
            m_Button.OnUp.AddListener(OnButtonUp);
            m_Button.OnUpAsButton.AddListener(OnButtonUpAsButton);
        }

        void OnDestroy()
        {
            m_Button.OnDown.RemoveAllListeners();
            m_Button.OnUp.RemoveAllListeners();
            m_Button.OnUpAsButton.RemoveAllListeners();
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

        void DoSetPosition(Vector2 position)
        {
            transform.position = position;
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
            ToggleOccupiable(false);
        }

        void ToggleOccupiable(bool active)
        {
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

        void OnButtonUp()
        {
            OnUp.Invoke(this);
        }

        void OnButtonUpAsButton()
        {
            OnUpAsButton.Invoke(this);
        }
    }
}