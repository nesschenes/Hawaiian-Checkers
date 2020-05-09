using UnityEngine;

namespace Hawaiian.Game
{
    public partial class Piece
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

            ClearEvents();
        }

        void DoSetName(string name)
        {
            gameObject.name = name;
        }

        void DoSetTeam(int team)
        {

        }

        void DoSetPosition(Vector2 position)
        {
            transform.position = position;
        }

        void DoSetPositionInTween(Vector2 position)
        {
            transform.position = position;
        }

        void DoSetColor(Color color)
        {
            m_Icon.color = color;
        }

        void ToggleIcon(bool active)
        {
            m_Icon.enabled = active;
        }

        void ToggleHighlight(bool active)
        {
            m_HighlightIcon.enabled = active;
        }

        void ToggleSelected(bool active)
        {
            m_SelectedIcon.enabled = active;
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