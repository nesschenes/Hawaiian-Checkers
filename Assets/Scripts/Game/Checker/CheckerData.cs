using UnityEngine;

namespace Hawaiian.Game
{
    public class CheckerData
    {
        public string Name;
        public Coordinate Coordinate;
        public int Team;
        public Vector2 Position;
        public Color Color;
    }

    public struct Coordinate
    {
        public readonly int X;
        public readonly int Y;

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Coordinate Zero { get; } = new Coordinate(0, 0);
        public static Coordinate One { get; } = new Coordinate(1, 1);
        public static Coordinate Left { get; } = new Coordinate(-1, 0);
        public static Coordinate Right { get; } = new Coordinate(1, 0);
        public static Coordinate Top { get; } = new Coordinate(0, 1);
        public static Coordinate Down { get; } = new Coordinate(0, -1);

        public static Coordinate operator +(Coordinate a, Coordinate b)
        {
            var coordinate = new Coordinate(a.X + b.X, a.Y + b.Y);
            return coordinate;
        }

        public static Coordinate operator -(Coordinate a, Coordinate b)
        {
            var coordinate = new Coordinate(a.X - b.X, a.Y - b.Y);
            return coordinate;
        }

        public static Coordinate operator *(Coordinate a, int b)
        {
            return new Coordinate(a.X * b, a.Y * b);
        }

        public static bool operator ==(Coordinate a, Coordinate b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Coordinate a, Coordinate b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        public bool Equals(Coordinate other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Coordinate other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}