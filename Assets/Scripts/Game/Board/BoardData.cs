using System;
using UnityEngine;

namespace Konane.Game
{
    [Serializable]
    public class BoardData : ICoordinateData
    {
        public string Name;
        public BoardState State;
        public Color Color;

        public Coordinate Coordinate { get; set; }
    }
}