using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Sprite
{
    public class Button : MonoBehaviour
    {
        public UnityEvent OnEnter = new UnityEvent();
        public UnityEvent OnExit = new UnityEvent();
        public UnityEvent OnDown = new UnityEvent();
        public UnityEvent OnDrag = new UnityEvent();
        public UnityEvent OnUp = new UnityEvent();
        public UnityEvent OnUpAsButton = new UnityEvent();

        void OnMouseEnter()
        {
            OnEnter.Invoke();
        }

        void OnMouseExit()
        {
            OnExit.Invoke();
        }

        void OnMouseDown()
        {
            OnDown.Invoke();
        }

        void OnMouseDrag()
        {
            OnDrag.Invoke();
        }

        void OnMouseUp()
        {
            OnUp.Invoke();
        }

        void OnMouseUpAsButton()
        {
            OnUpAsButton.Invoke();
        }
    }
}