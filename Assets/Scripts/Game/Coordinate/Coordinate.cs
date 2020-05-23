using System;

namespace Konane.Game
{
    /// <summary> Handle (x, y) that something like vector2 but integer </summary>
    [Serializable]
    public struct Coordinate
    {
        public int x;
        public int y;

        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Coordinate zero { get; } = new Coordinate(0, 0);
        public static Coordinate one { get; } = new Coordinate(1, 1);
        public static Coordinate left { get; } = new Coordinate(-1, 0);
        public static Coordinate right { get; } = new Coordinate(1, 0);
        public static Coordinate top { get; } = new Coordinate(0, 1);
        public static Coordinate down { get; } = new Coordinate(0, -1);

        public static Coordinate operator +(Coordinate a, Coordinate b)
        {
            var coordinate = new Coordinate(a.x + b.x, a.y + b.y);
            return coordinate;
        }

        public static Coordinate operator -(Coordinate a, Coordinate b)
        {
            var coordinate = new Coordinate(a.x - b.x, a.y - b.y);
            return coordinate;
        }

        public static Coordinate operator *(Coordinate a, int b)
        {
            return new Coordinate(a.x * b, a.y * b);
        }

        public static Coordinate operator /(Coordinate a, int b)
        {
            return new Coordinate(a.x / b, a.y / b);
        }

        public static bool operator ==(Coordinate a, Coordinate b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Coordinate a, Coordinate b)
        {
            return a.x != b.x || a.y != b.y;
        }

        public bool Equals(Coordinate other)
        {
            return x == other.x && y == other.y;
        }

        public override bool Equals(object obj)
        {
            return obj is Coordinate other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (x * 397) ^ y;
            }
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }
}