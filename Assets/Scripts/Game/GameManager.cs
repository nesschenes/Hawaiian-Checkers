using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.Game
{
    public partial class GameManager : MonoBehaviour
    {
        public Action OnRemoveStepDone = null;

        bool IsThisRoundOver => NextTurn < mCurrentTurn;
        int NextTurn => mCurrentTurn == mTurnCountPerRound ? 1 : mCurrentTurn + 1;

        public void Setup(int turnCountPerRound, int rowsCount)
        {
            mTurnCountPerRound = turnCountPerRound;
            mBoardRowsCount = rowsCount;
            mBoardGridCount = rowsCount * rowsCount;
            mBoards = new Board[mBoardGridCount];
            mPieces = new Piece[mBoardGridCount];

            var halfWidth = rowsCount / 2f;
            StartPosition = new Vector2(halfWidth * (-1) + 0.5f, -halfWidth + 0.5f); // left-top
            for (var i = 0; i < rowsCount; i++) // rows
            {
                for (var j = 0; j < rowsCount; j++) // columns
                {
                    var index = i * rowsCount + j;
                    var team = (i + j + 1) % 2 + 1; // 1, 2, 1, 2...
                    var coordinate = new Coordinate(j, i);

                    var boardData = new BoardData
                    {
                        Name = $"Board {coordinate.X} - {coordinate.Y}",
                        State = BoardState.None,
                        Coordinate = coordinate,
                        Color = m_BoardColor[team - 1]
                    };

                    var pieceData = new PieceData
                    {
                        Name = $"Piece {coordinate.X} - {coordinate.Y}",
                        Team = team,
                        State = PieceState.None,
                        Coordinate = coordinate,
                        Color = m_PieceColor[team - 1],
                    };

                    var piece = SpawnPiece(pieceData);
                    piece.SetAsNothingToDo();
                    var board = SpawnBoard(boardData);
                    board.SetPiece(piece);
                    mBoards[index] = board;
                    mPieces[index] = piece;
                }
            }

            Debug.LogFormat("Setup {0}x{0} Boards & Pieces", rowsCount);
        }

        public void DoRemoveStepJob()
        {
            mCurrentTurn = 0;
            DoNextRemoveTurnJob();
        }

        public void DoMoveStepJob()
        {
            mCurrentTurn = 0;
            DoNextMoveTurnJob();
        }

        void DoNextRemoveTurnJob()
        {
            ++mCurrentTurn;
            switch (mCurrentTurn)
            {
                case 1:
                {
                    SetPieceToRemovable(new Coordinate(0, mBoardRowsCount - 1));
                    SetPieceToRemovable(new Coordinate(mBoardRowsCount - 1, 0));
                    SetPieceToRemovable(new Coordinate(mBoardRowsCount / 2 - 1, mBoardRowsCount / 2));
                    SetPieceToRemovable(new Coordinate(mBoardRowsCount / 2, mBoardRowsCount / 2 - 1));
                }
                    break;
                case 2:
                {
                    SetAroundPieceToRemovable(mSelectedPiece);
                    mSelectedPiece = null;
                }
                    break;
            }

            Debug.LogFormat("Start Turn-Remove: {0} !", mCurrentTurn);
        }

        void DoNextMoveTurnJob()
        {
            var nextTurn = mCurrentTurn + 1;
            mCurrentTurn = nextTurn > mTurnCountPerRound ? 1 : nextTurn;
            var hasMovablePiece = false;
            foreach (var piece in mPieces)
            {
                if (piece.State == PieceState.Dead)
                    continue;

                if (mCurrentTurn != piece.Team)
                    continue;

                if (IsPieceMovable(piece.Coordinate))
                {
                    hasMovablePiece = true;
                    SetPieceToMovable(piece.Coordinate);
                }
            }

            Debug.LogFormat("Start Turn-Move: {0} !", mCurrentTurn);

            if (!hasMovablePiece)
                SetGameResult(mCurrentTurn, false);
        }

        void OnRemovablePieceSelected(Piece piece)
        {
            if (mSelectedPiece == piece)
            {
                mSelectedPiece.SetAsDead();

                if (IsThisRoundOver)
                {
                    SetPieceToNone(mPieces);
                    OnRemoveStepDone.Invoke();
                }
                else
                {
                    SetPieceToNone(mPieces);
                    DoNextRemoveTurnJob();
                }
            }
            else
            {
                if (mSelectedPiece != null)
                    mSelectedPiece.SetAsRemovable();

                mSelectedPiece = piece;
                mSelectedPiece.SetAsWaitToRemove();
            }
        }

        void OnMovablePieceSelected(Piece piece)
        {
            if (mSelectedPiece == piece)
                return;

            SetBoardToNone(mBoards);

            foreach (var p in mPieces)
            {
                if (p.State == PieceState.Movable)
                    continue;

                if (p.State == PieceState.WaitToMove)
                    p.SetAsMovable();
            }

            mSelectedPiece = piece;
            mSelectedPiece.SetAsWaitToMove();

            var coordinates = new List<Coordinate>();
            FindOccupiableCoordinate(mSelectedPiece.Coordinate, ref coordinates);
            foreach (var coordinate in coordinates)
                SetBoardToOccupiable(coordinate);
        }

        void OnOccupiableBoardSelected(Board board)
        {
            board.OnDown.RemoveListener(OnOccupiableBoardSelected);

            SetBoardToNone(mBoards);
            SetPieceToNone(mPieces);

            MovePieceToCoordinate(mSelectedPiece, board.Coordinate);

            DoNextMoveTurnJob();
        }

        void MovePieceToCoordinate(Piece piece, Coordinate target)
        {
            if (TryGetBoard(piece.Coordinate, out var fromBoard))
                fromBoard.SetPiece(null);

            var direction = (target - piece.Coordinate).Direction;
            if (TryGetBoard(piece.Coordinate + direction, out var crossedBoard))
                crossedBoard.Piece.SetAsDead();

            var nextCoordinate = piece.Coordinate + direction * 2;
            piece.SetCoordinateInTween(nextCoordinate);

            if (TryGetBoard(nextCoordinate, out var nextBoard))
                nextBoard.SetPiece(piece);

            if (nextCoordinate != target)
                MovePieceToCoordinate(piece, target);
        }

        void SetGameResult(int turn, bool win)
        {
            var message = win ? $"{turn} Win !" : $"{turn} Lose !";
            Debug.LogError(message);
        }
    }
}