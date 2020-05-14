using Konane.Renderer;
using UnityEngine;
using UnityEngine.Events;

namespace Konane.Game
{
    public abstract class CoordinateMono<T1, T2> : MonoBehaviour where T1 : ICoordinateEntity<T2> where T2 : ICoordinateData
    {
        [SerializeField]
        protected Button m_Button = null;
        [SerializeField]
        protected SpriteRenderer m_Icon = null;

        public Event OnDown = new Event();
        public Event OnDespawn = new Event();

        public class Event : UnityEvent<T1> { }

        protected virtual void Awake()
        {
            m_Button.OnDown.AddListener(OnButtonDown);
        }

        protected virtual void OnDestroy()
        {
            m_Button.OnDown.RemoveAllListeners();
        }

        public void SetInteractable(bool enable)
        {
            if (enable)
                m_Button.Active();
            else
                m_Button.Deactive();
        }

        protected abstract void OnButtonDown();
    }
}