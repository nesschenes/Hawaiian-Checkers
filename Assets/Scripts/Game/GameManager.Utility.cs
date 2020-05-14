using System.Collections.Generic;

namespace Konane.Game
{
    public partial class GameManager
    {
        Board SpawnBoard(BoardData data)
        {
            var board = Instantiate(m_Board, m_BoardPool);
            board.Init(data);
            return board;
        }

        Piece SpawnPiece(PieceData data)
        {
            var piece = Instantiate(m_Piece, m_PiecePool);
            piece.Init(data);
            return piece;
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
                if (board.Coordinate != coordinate) 
                    continue;

                result = board;
                return true;
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

                if (piece.Coordinate != coordinate) 
                    continue;

                result = piece;
                return true;
            }

            result = null;
            return false;
        }

        void SetBoardToNone(Board[] boards)
        {
            foreach (var board in boards)
            {
                board.ClearInputEvents();
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

            piece.OnDown.AddListener(OnRemovablePieceSelected);
            piece.SetAsRemovable();
        }

        void SetPieceToMovable(Coordinate coordinate)
        {
            if (!TryGetPiece(coordinate, out var piece))
                return;

            piece.OnDown.AddListener(OnMovablePieceSelected);
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

        void FindAllOccupiableAndEatablePieces(Coordinate coordinate, ref Dictionary<Coordinate, Queue<Coordinate>> occupiable, ref Dictionary<Coordinate, Piece> eatable)
        {
            FindEatablePieces(coordinate, Coordinate.Top, ref occupiable, ref eatable);
            FindEatablePieces(coordinate, Coordinate.Down, ref occupiable, ref eatable);
            FindEatablePieces(coordinate, Coordinate.Left, ref occupiable, ref eatable);
            FindEatablePieces(coordinate, Coordinate.Right, ref occupiable, ref eatable);
        }

        void FindEatablePieces(Coordinate coordinate, Coordinate direction, ref Dictionary<Coordinate, Queue<Coordinate>> occupiable, ref Dictionary<Coordinate, Piece> eatable)
        {
            var eatableCoordinate = coordinate + direction;
            var occupiableCoordinate = coordinate + direction * 2;
            if (!IsValid(occupiableCoordinate)
                || HasPiece(occupiableCoordinate)
                || !TryGetPiece(eatableCoordinate, out var piece))
                return;

            var list = occupiable.TryGetValue(coordinate, out var lastResult)
                           ? new Queue<Coordinate>(lastResult)
                           : new Queue<Coordinate>();
            list.Enqueue(occupiableCoordinate);

            eatable.Add(occupiableCoordinate, piece);
            occupiable.Add(occupiableCoordinate, list);
            FindEatablePieces(occupiableCoordinate, direction, ref occupiable, ref eatable);
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