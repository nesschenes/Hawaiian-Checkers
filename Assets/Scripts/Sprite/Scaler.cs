using Hawaiian.Utility;
using System;
using UnityEngine;

namespace Hawaiian.Sprite
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Scaler : MonoBehaviour
    {
        SpriteRenderer mRenderer = null;

        void Start()
        {
            mRenderer = gameObject.GetComponent<SpriteRenderer>();
            SetFullScreen();
        }

        public void SetFullScreen()
        {
            mRenderer.size = new Vector2(ScreenUtility.Width, ScreenUtility.Height);
        }
    }
}