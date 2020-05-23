using System;
using UnityEngine;

namespace Konane.Game
{
    [Serializable]
    public class PieceData : ICoordinateData
    {
        public string Name;
        public int Team;
        public PieceState State;
        public Color Color;

        public Coordinate Coordinate { get; set; }
        public Coordinate LastCoordinate { get; set; }
    }
}