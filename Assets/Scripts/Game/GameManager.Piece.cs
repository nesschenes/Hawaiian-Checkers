using System;
using System.Collections.Generic;

namespace Konane.Game
{
    public partial class GameManager
    {
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

        void SetPieceToNone(Piece[] pieces)
        {
            foreach (var piece in pieces)
            {
                piece.ClearEvents();

                if (piece.State != PieceState.Dead)
                    piece.SetAsNothingToDo();
            }
        }

        void SetAroundPieceToRemovable(Coordinate coordinate)
        {
            SetPieceToRemovable(coordinate + Coordinate.top);
            SetPieceToRemovable(coordinate + Coordinate.down);
            SetPieceToRemovable(coordinate + Coordinate.left);
            SetPieceToRemovable(coordinate + Coordinate.right);
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
            var topCoordinate = coordinate + Coordinate.top * 2;
            if (IsValid(topCoordinate) && !HasPiece(topCoordinate) && HasPiece(coordinate + Coordinate.top))
                return true;

            var downCoordinate = coordinate + Coordinate.down * 2;
            if (IsValid(downCoordinate) && !HasPiece(downCoordinate) && HasPiece(coordinate + Coordinate.down))
                return true;

            var leftCoordinate = coordinate + Coordinate.left * 2;
            if (IsValid(leftCoordinate) && !HasPiece(leftCoordinate) && HasPiece(coordinate + Coordinate.left))
                return true;

            var rightCoordinate = coordinate + Coordinate.right * 2;
            if (IsValid(rightCoordinate) && !HasPiece(rightCoordinate) && HasPiece(coordinate + Coordinate.right))
                return true;

            return false;
        }

        void MovePieceToCoordinates(Piece piece, Queue<Coordinate> coordinates, Action onComplete)
        {
            if (coordinates.Count == 0)
            {
                onComplete?.Invoke();
                return;
            }

            if (TryGetBoard(piece.Coordinate, out var board))
                board.SetPiece(null);

            var coordinate = coordinates.Dequeue();
            piece.SetCoordinateInTween(coordinate,
                                       () =>
                                       {
                                           OnPieceMoveComplete(piece);
                                           MovePieceToCoordinates(piece, coordinates, onComplete);
                                       });
        }

        void FindAllOccupiableAndEatablePieces(Coordinate coordinate, ref Dictionary<Coordinate, Queue<Coordinate>> occupiable, ref Dictionary<Coordinate, Piece> eatable)
        {
            FindEatablePieces(coordinate, Coordinate.top, ref occupiable, ref eatable);
            FindEatablePieces(coordinate, Coordinate.down, ref occupiable, ref eatable);
            FindEatablePieces(coordinate, Coordinate.left, ref occupiable, ref eatable);
            FindEatablePieces(coordinate, Coordinate.right, ref occupiable, ref eatable);
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
    }
}