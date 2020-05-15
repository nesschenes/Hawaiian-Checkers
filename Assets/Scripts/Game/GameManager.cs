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

        public Action OnRemoveStepDone = delegate { };
        public Action OnMoveStepDone = delegate { };

        bool IsThisRoundOver => NextPieceType < mCurrentPieceType;
        int NextPieceType => mCurrentPieceType == mPieceTypeCount ? 1 : mCurrentPieceType + 1;

        /// <summary> Generate the boards and pieces according to GameSettings </summary>
        public void Generate()
        {
            mPieceTypeCount = GameSettings.PieceTypeCount;
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

                    var boardData = Generate<BoardData>(coordinate);
                    boardData.Name = $"Board {coordinate.x} - {coordinate.y}";
                    boardData.State = BoardState.None;
                    boardData.Color = m_BoardColor[team - 1];

                    var pieceData = Generate<PieceData>(coordinate);
                    pieceData.LastCoordinate = coordinate;
                    pieceData.Name = $"Piece {coordinate.x} - {coordinate.y}";
                    pieceData.Team = team;
                    pieceData.State = PieceState.None;
                    pieceData.Color = m_PieceColor[team - 1];

                    var board = Generate(m_Board, boardData, m_BoardPool);
                    var piece = Generate(m_Piece, pieceData, m_PiecePool);
                    board.SetPiece(piece);
                    mBoards[index] = board;
                    mPieces[index] = piece;
                }
            }

            Debug.LogFormat("Generated {0}x{0} Boards & Pieces", mBoardRowsCount);
        }

        public void DoRemoveStepJob()
        {
            mCurrentPieceType = 0;
            DoNextRemoveTurnJob();
        }

        public void DoMoveStepJob()
        {
            mCurrentPieceType = 0;
            DoNextMoveTurnJob();
        }

        void DoNextRemoveTurnJob()
        {
            ++mCurrentPieceType;
            switch (mCurrentPieceType)
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

            Debug.LogFormat("Start {0} Remove-Turn", mCurrentPieceType);
        }

        void DoNextMoveTurnJob()
        {
            mCurrentPieceType = NextPieceType;
            var hasMovablePiece = false;
            foreach (var piece in mPieces)
            {
                if (piece.State == PieceState.Dead)
                    continue;

                if (mCurrentPieceType != piece.Team)
                    continue;

                if (!IsPieceMovable(piece.Coordinate)) 
                    continue;

                hasMovablePiece = true;
                SetPieceToMovable(piece.Coordinate);
            }

            Debug.LogFormat("Start {0} Move-Turn", mCurrentPieceType);

            if (!hasMovablePiece)
            {
                Winner = NextPieceType;
                OnMoveStepDone();
            }
        }

        void OnRemovablePieceSelected(Piece piece)
        {
            // confirm to remove selected piece
            if (mSelectedPiece == piece)
            {
                mSelectedPiece.SetAsDead();

                // set all piece to none state and clear events
                SetPieceToNone(mPieces);

                if (IsThisRoundOver)
                    OnRemoveStepDone();
                else
                    DoNextRemoveTurnJob();
            }
            else
            {
                // change last selected piece from WaitToRemove to Removable because another piece is selected now
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

        void OnPieceMoveComplete(Piece piece)
        {
            if (mEatablePieceDict.TryGetValue(piece.Coordinate, out var eatablePiece))
                eatablePiece.SetAsDead();

            if (TryGetBoard(piece.Coordinate, out var board))
                board.SetPiece(piece);
        }
    }
}