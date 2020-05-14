using UnityEngine;

namespace Konane.Game
{
    public class BoardData : ICoordinateData
    {
        public string Name;
        public BoardState State;
        public Color Color;

        public Coordinate Coordinate { get; set; }
    }
}