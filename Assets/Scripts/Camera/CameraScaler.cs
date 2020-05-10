using Konane.Game;
using Konane.Utility;
using UnityEngine;

namespace Konane.RWD
{
    public class CameraScaler : MonoBehaviour
    {
        void Awake()
        {
            SetWidth(GameSettings.BoardRowsCount);
            Notify.RefreshScaler += OnRefreshScaler;
        }

        void OnDestroy()
        {
            Notify.RefreshScaler -= OnRefreshScaler;
        }

        void SetWidth(float width)
        {
            var screenRatio = ScreenUtility.Ratio;
            var targetRaito = 1; // checkerboard is square shape.
            if (screenRatio > targetRaito)
                Camera.main.orthographicSize = width * targetRaito * 0.5f;               // match height
            else
                Camera.main.orthographicSize = width * targetRaito / screenRatio * 0.5f; // match width
        }

        void OnRefreshScaler()
        {
            SetWidth(GameSettings.BoardRowsCount);
        }
    }
}