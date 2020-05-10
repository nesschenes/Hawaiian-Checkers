using Konane.Utility;
using UnityEngine;

namespace Konane.RWD
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteScaler : MonoBehaviour
    {
        SpriteRenderer mRenderer = null;

        void Awake()
        {
            Notify.RefreshScaler += OnRefreshScaler;
        }

        void Start()
        {
            mRenderer = gameObject.GetComponent<SpriteRenderer>();
            SetFullScreen();
        }

        void OnDestroy()
        {
            Notify.RefreshScaler -= OnRefreshScaler;
        }

        void SetFullScreen()
        {
            mRenderer.size = new Vector2(ScreenUtility.Width, ScreenUtility.Height);
        }

        void OnRefreshScaler()
        {
            SetFullScreen();
        }
    }
}