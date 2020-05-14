using System;

namespace Konane.Game
{
    public interface ICoordinateTween
    {
        Coordinate Coordinate { get; set; }
        Action OnComplete { get; set; }
    }
}