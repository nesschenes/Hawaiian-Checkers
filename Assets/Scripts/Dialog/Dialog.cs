using System;
using UnityEngine;
using UnityEngine.UI;

namespace Konane
{
    public class Dialog : MonoSingleton<Dialog>
    {
        [SerializeField]
        Canvas m_Canvas = null;
        [SerializeField]
        GraphicRaycaster m_GraphicRaycaster = null;
        [SerializeField]
        Text m_Title = null;
        [SerializeField]
        Button m_Confirm = null;
        [SerializeField]
        Button m_Cancel = null;
        [SerializeField]
        Button m_Background = null;

        Action Confirm = delegate { };
        Action Cancel = delegate { };

        protected override bool Immortal => true;

        protected override void Awake()
        {
            base.Awake();

            m_Confirm.onClick.AddListener(OnConfirm);
            m_Cancel.onClick.AddListener(OnCancel);
            m_Background.onClick.AddListener(OnCancel);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            m_Confirm.onClick.RemoveListener(OnConfirm);
            m_Cancel.onClick.RemoveListener(OnCancel);
            m_Background.onClick.RemoveListener(OnCancel);
        }

        public void Show(string title, Action onConfirm, Action onCancel)
        {
            m_Title.text = title;
            m_Canvas.enabled = true;
            m_GraphicRaycaster.enabled = true;

            Confirm = onConfirm;
            Cancel = onCancel;
        }

        public void Hide()
        {
            m_Canvas.enabled = false;
            m_GraphicRaycaster.enabled = false;
        }

        void OnConfirm()
        {
            Hide();

            Confirm?.Invoke();
        }

        void OnCancel()
        {
            Hide();

            Cancel?.Invoke();
        }
    }
}