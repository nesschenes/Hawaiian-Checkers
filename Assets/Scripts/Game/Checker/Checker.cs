using Hawaiian.Sprite;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Game
{
    public enum CheckerState
    {
        NothingToDo = 0,
        Removable = 1,
        WaitToRemove = 2,
        Movable = 3,
        WaitToMove = 4,
        Dead = 999,
    }

    public class Checker : MonoBehaviour
    {
        [SerializeField]
        SpriteRenderer m_Icon = null;
        [SerializeField]
        SpriteRenderer m_HighlightIcon = null;
        [SerializeField]
        SpriteRenderer m_SelectedIcon = null;
        [SerializeField]
        Button m_Button = null;

        public CheckerData Data { get; private set; }
        public CheckerState State { get; private set; }

        public CheckerEvent OnDown = new CheckerEvent();
        public CheckerEvent OnUp = new CheckerEvent();
        public CheckerEvent OnUpAsButton = new CheckerEvent();

        public class CheckerEvent : UnityEvent<Checker> { }

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
            OnDown.RemoveAllListeners();
            OnUp.RemoveAllListeners();
            OnUpAsButton.RemoveAllListeners();
        }

        public void Init(CheckerData data)
        {
            Data = data;

            SetName(data.Name);
            SetTeam(data.Team);
            SetPosition(data.Position);
            SetColor(data.Color);
        }

        public void SetAsNothingToDo()
        {
            State = CheckerState.NothingToDo;
            OnUpAsButton.RemoveAllListeners();
            SetHighlight(false);
        }

        public void SetAsRemovable()
        {
            State = CheckerState.Removable;
            SetHighlight(true);
            SetSelected(false);
        }

        public void SetAsWaitToRemove()
        {
            State = CheckerState.WaitToRemove;
            SetHighlight(false);
            SetSelected(true);
        }

        public void SetAsMovable()
        {
            State = CheckerState.Movable;
            SetHighlight(true);
            SetSelected(false);
        }

        public void SetAsWaitToMove()
        {
            State = CheckerState.WaitToMove;
            SetHighlight(false);
            SetSelected(true);
        }

        public void SetAsDead()
        {
            State = CheckerState.Dead;
            gameObject.SetActive(false);
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
            m_HighlightIcon.enabled = active;
        }

        public void SetSelected(bool active)
        {
            m_SelectedIcon.enabled = active;
        }

        public void Dispose()
        {
            gameObject.SetActive(false);
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