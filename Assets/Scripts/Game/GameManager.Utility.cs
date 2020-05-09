using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.Game
{
    public partial class GameManager
    {
        public static Vector2 StartPosition { get; private set; }

        public static Vector2 ConvertToPosition(Coordinate coordinate)
        {
            return StartPosition + new Vector2(coordinate.X, coordinate.Y);
        }

        Board SpawnBoard(BoardData data)
        {
            var boardUnit = Instantiate(m_Board, m_BoardPool);
            boardUnit.Init(data);
            return boardUnit;
        }

        Piece SpawnPiece(PieceData data)
        {
            var checker = Instantiate(m_Piece, m_PiecePool);
            checker.Init(data);
            return checker;
        }

        bool TryGetBoard(Coordinate coordinate, out Board result)
        {
            if (!IsValid(coordinate))
            {
                result = null;
                return false;
            }

            foreach (var board in mBoards)
            {
                if (board.Coordinate == coordinate)
                {
                    result = board;
                    return true;
                }
            }

            result = null;
            return false;
        }

        bool TryGetPiece(Coordinate coordinate, out Piece result)
        {
            if (!IsValid(coordinate))
            {
                result = null;
                return false;
            }

            foreach (var piece in mPieces)
            {
                if (piece.State == PieceState.Dead)
                    continue;

                if (piece.Coordinate == coordinate)
                {
                    result = piece;
                    return true;
                }
            }

            result = null;
            return false;
        }

        void SetBoardToNone(Board[] boards)
        {
            foreach (var board in boards)
            {
                board.ClearEvents();
                board.SetState(BoardState.None);
            }
        }

        void SetBoardToOccupiable(Coordinate coordinate)
        {
            if (!TryGetBoard(coordinate, out var board))
                return;

            board.OnDown.AddListener(OnOccupiableBoardSelected);
            board.SetState(BoardState.Occupiable);
        }

        void SetPieceToNone(Piece[] pieces)
        {
            foreach (var piece in pieces)
            {
                piece.ClearEvents();

                if (piece.State != PieceState.Dead)
                    piece.SetAsNothingToDo();
            }
        }

        void SetAroundPieceToRemovable(Piece piece)
        {
            var coordinate = piece.Coordinate;
            SetPieceToRemovable(coordinate + Coordinate.Top);
            SetPieceToRemovable(coordinate + Coordinate.Down);
            SetPieceToRemovable(coordinate + Coordinate.Left);
            SetPieceToRemovable(coordinate + Coordinate.Right);
        }

        void SetPieceToRemovable(Coordinate coordinate)
        {
            if (!TryGetPiece(coordinate, out var piece))
                return;

            piece.OnUpAsButton.AddListener(OnRemovablePieceSelected);
            piece.SetAsRemovable();
        }

        void SetPieceToMovable(Coordinate coordinate)
        {
            if (!TryGetPiece(coordinate, out var piece))
                return;

            piece.OnUpAsButton.AddListener(OnMovablePieceSelected);
            piece.SetAsMovable();
        }

        bool IsPieceMovable(Coordinate coordinate)
        {
            var topCoordinate = coordinate + Coordinate.Top * 2;
            if (IsValid(topCoordinate) && !HasPiece(topCoordinate) && HasPiece(coordinate + Coordinate.Top))
                return true;

            var downCoordinate = coordinate + Coordinate.Down * 2;
            if (IsValid(downCoordinate) && !HasPiece(downCoordinate) && HasPiece(coordinate + Coordinate.Down))
                return true;

            var leftCoordinate = coordinate + Coordinate.Left * 2;
            if (IsValid(leftCoordinate) && !HasPiece(leftCoordinate) && HasPiece(coordinate + Coordinate.Left))
                return true;

            var rightCoordinate = coordinate + Coordinate.Right * 2;
            if (IsValid(rightCoordinate) && !HasPiece(rightCoordinate) && HasPiece(coordinate + Coordinate.Right))
                return true;

            return false;
        }

        void FindTopOccupiableCoordinate(Coordinate coordinate, ref List<Coordinate> coordinates)
        {
            var topCoordinate = coordinate + Coordinate.Top * 2;
            if (IsValid(topCoordinate) && !HasPiece(topCoordinate) && HasPiece(coordinate + Coordinate.Top))
            {
                coordinates.Add(topCoordinate);
                FindTopOccupiableCoordinate(topCoordinate, ref coordinates);
            }
        }

        void FindDownOccupiableCoordinate(Coordinate coordinate, ref List<Coordinate> coordinates)
        {
            var downCoordinate = coordinate + Coordinate.Down * 2;
            if (IsValid(downCoordinate) && !HasPiece(downCoordinate) && HasPiece(coordinate + Coordinate.Down))
            {
                coordinates.Add(downCoordinate);
                FindDownOccupiableCoordinate(downCoordinate, ref coordinates);
            }
        }

        void FindLeftOccupiableCoordinate(Coordinate coordinate, ref List<Coordinate> coordinates)
        {
            var leftCoordinate = coordinate + Coordinate.Left * 2;
            if (IsValid(leftCoordinate) && !HasPiece(leftCoordinate) && HasPiece(coordinate + Coordinate.Left))
            {
                coordinates.Add(leftCoordinate);
                FindLeftOccupiableCoordinate(leftCoordinate, ref coordinates);
            }
        }

        void FindRightOccupiableCoordinate(Coordinate coordinate, ref List<Coordinate> coordinates)
        {
            var rightCoordinate = coordinate + Coordinate.Right * 2;
            if (IsValid(rightCoordinate) && !HasPiece(rightCoordinate) && HasPiece(coordinate + Coordinate.Right))
            {
                coordinates.Add(rightCoordinate);
                FindRightOccupiableCoordinate(rightCoordinate, ref coordinates);
            }
        }

        void FindOccupiableCoordinate(Coordinate coordinate, ref List<Coordinate> coordinates)
        {
            FindTopOccupiableCoordinate(coordinate, ref coordinates);
            FindDownOccupiableCoordinate(coordinate, ref coordinates);
            FindLeftOccupiableCoordinate(coordinate, ref coordinates);
            FindRightOccupiableCoordinate(coordinate, ref coordinates);
        }

        bool IsValid(Coordinate coordinate)
        {
            return IsValid(coordinate.X, coordinate.Y);
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

        bool HasPiece(Coordinate coordinate)
        {
            foreach (var board in mBoards)
                if (board.Coordinate == coordinate)
                    return board.HasPiece;

            return false;
        }
    }
}