using UnityEngine;

namespace Konane.Game
{
    public class PieceData
    {
        public string Name;
        public int Team;
        public PieceState State;
        public Coordinate Coordinate;
        public Coordinate LastCoordinate;
        public Color Color;
    }

    public struct Coordinate
    {
        public readonly int X;
        public readonly int Y;
        public int Length => X + Y;
        public Coordinate Direction => new Coordinate(NoramlizeX(), NoramlizeY());

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        int NoramlizeX()
        {
            if (X > 0)
                return 1;
            else if (X < 0)
                return -1;
            else
                return 0;
        }

        int NoramlizeY()
        {
            if (Y > 0)
                return 1;
            else if (Y < 0)
                return -1;
            else
                return 0;
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

        public static Coordinate operator /(Coordinate a, int b)
        {
            return new Coordinate(a.X / b, a.Y / b);
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