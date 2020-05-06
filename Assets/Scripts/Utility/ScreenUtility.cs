using UnityEngine;

namespace Hawaiian.Utility
{
    public static class ScreenUtility
    {
        public static float Width => UnityEngine.Camera.main.ViewportToWorldPoint(new Vector2(1, 1)).x * 2;

        public static float Height => UnityEngine.Camera.main.ViewportToWorldPoint(new Vector2(1, 1)).y * 2;

        /// <summary> Width / Height </summary>
        public static float Ratio => (float)Screen.width / Screen.height;
    }
}