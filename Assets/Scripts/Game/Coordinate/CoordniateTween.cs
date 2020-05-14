using System;

namespace Konane.Game
{
    public class CoordniateTween : ICoordinateTween
    {
        public Coordinate Coordinate { get; set; }
        public Action OnComplete { get; set; }

        public CoordniateTween(Coordinate coordinate, Action onComplete)
        {
            Coordinate = coordinate;
            OnComplete = onComplete;
        }
    }
}