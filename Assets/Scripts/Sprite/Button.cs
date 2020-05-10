using UnityEngine;
using UnityEngine.Events;

namespace Konane.Renderer
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public class Button : MonoBehaviour
    {
        public UnityEvent OnEnter = new UnityEvent();
        public UnityEvent OnExit = new UnityEvent();
        public UnityEvent OnDown = new UnityEvent();
        public UnityEvent OnDrag = new UnityEvent();
        public UnityEvent OnUp = new UnityEvent();
        public UnityEvent OnUpAsButton = new UnityEvent();

        Collider2D mCollider = null;

        void Awake()
        {
            mCollider = gameObject.GetComponent<Collider2D>();
        }

        public void Active()
        {
            mCollider.enabled = true;
        }

        public void Deactive()
        {
            mCollider.enabled = false;
        }

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