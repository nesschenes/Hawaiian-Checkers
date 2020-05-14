using UnityEngine;

namespace Konane.Game
{
    public partial class GameManager
    {
        T Generate<T>(Coordinate coordinate)
            where T : ICoordinateData, new()
        {
            var data = new T
            {
                Coordinate = coordinate
            };
            return data;
        }

        T1 Generate<T1, T2>(T1 source, T2 data, Transform pool)
            where T1 : MonoBehaviour, ICoordinateEntity<T2>
            where T2 : ICoordinateData
        {
            var entity = Instantiate(source, pool);
            entity.Init(data);
            return entity;
        }

        bool IsValid(Coordinate coordinate)
        {
            return IsValid(coordinate.x, coordinate.y);
        }

        bool IsValid(int x, int y)
        {
            if (x < 0)
                return false;
            if (x >= mBoardRowsCount)
                return false;
            if (y < 0)
                return false;
            if (y >= mBoardRowsCount)
                return false;

            return true;
        }
    }
}