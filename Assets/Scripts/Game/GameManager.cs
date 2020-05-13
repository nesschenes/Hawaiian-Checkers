using System;
using System.Collections.Generic;
using UnityEngine;

namespace Konane.Game
{
    /// <summary> The Manager of Boards and Pieces </summary>
    public partial class GameManager : MonoSingleton<GameManager>
    {
        /// <summary> the starting point in world space </summary>
        public Vector2 BoardStartPosition { get; private set; }
        public int Winner { get; private set; }

        public Action OnRemoveStepDone = null;
        public Action OnMoveStepDone = null;

        bool IsThisRoundOver => NextPieceType < mCurrentPieceTeam;
        int NextPieceType => mCurrentPieceTeam == mPieceTeamCount ? 1 : mCurrentPieceTeam + 1;

        /// <summary> mapping coordinate(if the piece move to here) to the paths of coordinate </summary>
        Dictionary<Coordinate, Queue<Coordinate>> mOccupiablePathDict = new Dictionary<Coordinate, Queue<Coordinate>>(8);
        /// <summary> mapping coordinate(if the piece move to here) to the coordinate be eaten </summary
        Dictionary<Coordinate, Piece> mEatablePieceDict = new Dictionary<Coordinate, Piece>();

        public void Generate()
        {
            mPieceTeamCount = GameSettings.PieceTypeCount;
            mBoardRowsCount = GameSettings.BoardRowsCount;
            mBoardGridCount = mBoardRowsCount * mBoardRowsCount;
            mBoards = new Board[mBoardGridCount];
            mPieces = new Piece[mBoardGridCount];

            var halfWidth = mBoardRowsCount / 2f;
            BoardStartPosition = new Vector2(halfWidth * (-1) + 0.5f, -halfWidth + 0.5f); // left-top

            for (var i = 0; i < mBoardRowsCount; i++) // rows
            {
                for (var j = 0; j < mBoardRowsCount; j++) // columns
                {
                    var index = i * mBoardRowsCount + j;
                    var team = (i + j + 1 + GameSettings.PieceTypeToBegin) % 2 + 1; // 1, 2, 1, 2...
                    var coordinate = new Coordinate(j, i); // from (0, 0) to (boardSize, boardSize)

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
                        LastCoordinate = coordinate,
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

            Debug.LogFormat("Generate {0}x{0} Boards & Pieces", mBoardRowsCount);
        }

        public void DoRemoveStepJob()
        {
            mCurrentPieceTeam = 0;
            DoNextRemoveTurnJob();
        }

        public void DoMoveStepJob()
        {
            mCurrentPieceTeam = 0;
            DoNextMoveTurnJob();
        }

        void DoNextRemoveTurnJob()
        {
            ++mCurrentPieceTeam;
            switch (mCurrentPieceTeam)
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

            Debug.LogFormat("Start {0} Remove-Turn", mCurrentPieceTeam);
        }

        void DoNextMoveTurnJob()
        {
            mCurrentPieceTeam = NextPieceType;
            var hasMovablePiece = false;
            foreach (var piece in mPieces)
            {
                if (piece.State == PieceState.Dead)
                    continue;

                if (mCurrentPieceTeam != piece.Team)
                    continue;

                if (IsPieceMovable(piece.Coordinate))
                {
                    hasMovablePiece = true;
                    SetPieceToMovable(piece.Coordinate);
                }
            }

            Debug.LogFormat("Start {0} Move-Turn", mCurrentPieceTeam);

            if (!hasMovablePiece)
            {
                Winner = NextPieceType;
                OnMoveStepDone.Invoke();
            }
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

            mOccupiablePathDict.Clear();
            mEatablePieceDict.Clear();
            FindAllOccupiableAndEatablePieces(mSelectedPiece.Coordinate, ref mOccupiablePathDict, ref mEatablePieceDict);
            foreach (var coordinate in mOccupiablePathDict.Keys)
                SetBoardToOccupiable(coordinate);
        }

        void OnOccupiableBoardSelected(Board board)
        {
            board.OnDown.RemoveListener(OnOccupiableBoardSelected);

            SetBoardToNone(mBoards);
            SetPieceToNone(mPieces);

            var coordinates = mOccupiablePathDict.TryGetValue(board.Coordinate, out var result) 
                                  ? result
                                  : new Queue<Coordinate>();
            MovePieceToCoordinates(mSelectedPiece, coordinates, DoNextMoveTurnJob);
        }

        void MovePieceToCoordinates(Piece piece, Queue<Coordinate> coordinates, Action onDone)
        {
            if (coordinates.Count == 0)
            {
                onDone?.Invoke();
                return;
            }

            if (TryGetBoard(piece.Coordinate, out var board))
                board.SetPiece(null);

            var coordinate = coordinates.Dequeue();
            piece.SetCoordinateInTween(coordinate,
                                       () =>
                                       {
                                           OnPieceMoveComplete(piece);
                                           MovePieceToCoordinates(piece, coordinates, onDone);
                                       });
        }

        void OnPieceMoveComplete(Piece piece)
        {
            if (mEatablePieceDict.TryGetValue(piece.Coordinate, out var eatablePiece))
                eatablePiece.SetAsDead();

            if (TryGetBoard(piece.Coordinate, out var board))
                board.SetPiece(piece);
        }
    }
}