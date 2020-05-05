using UnityEngine;

namespace Hawaiian.Game
{
    public class Checker : MonoBehaviour
    {
        [SerializeField]
        SpriteRenderer m_Icon = null;
        [SerializeField]
        SpriteRenderer m_Highlight = null;

        public CheckerData Data { get; private set; }
        public bool Interactable { get; private set; }

        public void Init(CheckerData data)
        {
            Data = data;

            SetName(data.Name);
            SetTeam(data.Team);
            SetPosition(data.Position);
            SetColor(data.Color);
        }

        public void Refresh()
        {
            SetHighlight(Interactable);
        }

        public void SetRemovable()
        {
            Interactable = true;
            SetHighlight(true);
        }

        public void SetName(string name)
        {
            gameObject.name = name;
        }

        public void SetTeam(int team)
        {

        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void SetColor(Color color)
        {
            m_Icon.color = color;
        }

        public void SetHighlight(bool active)
        {
            m_Highlight.enabled = active;
        }

        public void Dispose()
        {
            gameObject.SetActive(false);
        }
    }
}