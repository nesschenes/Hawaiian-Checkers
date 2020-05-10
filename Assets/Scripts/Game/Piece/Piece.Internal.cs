using UnityEngine;

namespace Konane.Game
{
    public partial class Piece
    {
        void Awake()
        {
            m_Button.OnDown.AddListener(OnButtonDown);
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

        void DoSetCooridinate(Coordinate coordinate)
        {
            transform.position = GameUtility.CoordinateToPosition(coordinate);
        }

        void DoSetCooridinateInTween(Coordinate coordinate)
        {
            transform.position = GameUtility.CoordinateToPosition(coordinate);
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
    }
}