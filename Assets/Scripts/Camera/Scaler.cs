using Hawaiian.Utility;
using UnityEngine;

namespace Hawaiian.Camera
{
    public class Scaler : MonoBehaviour
    {
        void Awake()
        {
            SetWidth(6);
        }

        void SetWidth(float width)
        {
            var screenRatio = ScreenUtility.Ratio;
            var targetRaito = 1; // checkerboard is square shape.
            if (screenRatio > targetRaito)
                UnityEngine.Camera.main.orthographicSize = width * targetRaito * 0.5f;               // match height
            else
                UnityEngine.Camera.main.orthographicSize = width * targetRaito / screenRatio * 0.5f; // match width
        }
    }
}