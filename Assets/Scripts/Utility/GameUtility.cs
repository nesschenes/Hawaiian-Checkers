using UnityEngine;

namespace Konane.Game
{
    public static class GameUtility
    {
        public static Vector2 CoordinateToPosition(Coordinate coordinate)
        {
            if (!GameManager.IsExist)
            {
                Debug.LogErrorFormat("{0} isn't exist !", nameof(GameManager));
                return Vector2.zero;
            }

            return GameManager.Instance.BoardStartPosition + new Vector2(coordinate.x, coordinate.y);
        }
    }
}