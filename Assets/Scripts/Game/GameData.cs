using System;
using UnityEngine;

namespace Konane.Game
{
    [Serializable]
    public class GameData
    {
        public GameStep GameStep;
        public int CurrentPieceType = 1;
        public int PieceTypeCount = 2;
        public int BoardRowsCount = 6;
        public BoardRawData[] BoardData;
        public PieceRawData[] PieceData;

        public static GameData LastData = null;
    }

    [Serializable]
    public class BoardRawData
    {
        public string Name;
        public BoardState State;
        public Color Color;
        public Coordinate Coordinate;

        public static BoardRawData Convert(BoardData data)
        {
            return new BoardRawData
            {
                Name = data.Name,
                State = data.State,
                Color = data.Color,
                Coordinate = data.Coordinate
            };
        }
    }

    [Serializable]
    public class PieceRawData
    {
        public string Name;
        public int Team;
        public PieceState State;
        public Color Color;
        public Coordinate Coordinate;

        public static PieceRawData Convert(PieceData data)
        {
            return new PieceRawData
            {
                Name = data.Name,
                Team = data.Team,
                State = data.State,
                Color = data.Color,
                Coordinate = data.Coordinate
            };
        }
    }
}
