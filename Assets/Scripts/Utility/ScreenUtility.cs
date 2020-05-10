using UnityEngine;

namespace Konane.Utility
{
    public static class ScreenUtility
    {
        public static float Width => Camera.main.ViewportToWorldPoint(Vector2.one).x * 2;

        public static float Height => Camera.main.ViewportToWorldPoint(Vector2.one).y * 2;

        /// <summary> Width / Height </summary>
        public static float Ratio => (float)Screen.width / Screen.height;
    }
}