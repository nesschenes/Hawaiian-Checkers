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
            BoardStartPosition = new Vector2(halfWidth * (-1) + 0.5f, -halfWidth + 0.5f); // left-down

            for (var i = 0; i < mBoardRowsCount; i++) // rows
            {
                for (var j = 0; j < mBoardRowsCount; j++) // columns
                {
                    var index = i * mBoardRowsCount + j;
                    var team = (i + j + GameSettings.PieceTypeToBegin) % 2 + 1; // 1, 2, 1, 2...
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

        /// <summary> Resume the boards and pieces according to GameData </summary>
        public void Resume(GameData data)
        {
            mCurrentPieceType = data.CurrentPieceType;
            mBoardRowsCount = data.BoardRowsCount;
            mBoardGridCount = mBoardRowsCount * mBoardRowsCount;

            var halfWidth = mBoardRowsCount / 2f;
            BoardStartPosition = new Vector2(halfWidth * (-1) + 0.5f, -halfWidth + 0.5f); // left-down

            var boardCount = data.BoardData.Length;
            mBoards = new Board[boardCount];
            for (var i = 0; i < mBoards.Length; i++)
            {
                var rawData = data.BoardData[i];
                var boardData = Generate<BoardData>(rawData.Coordinate);
                boardData.Name = rawData.Name;
                boardData.State = rawData.State;
                boardData.Color = rawData.Color;
                mBoards[i] = Generate(m_Board, boardData, m_BoardPool);
            }

            var pieceCount = data.PieceData.Length;
            mPieces = new Piece[pieceCount];
            for (var i = 0; i < mPieces.Length; i++)
            {
                var rawData = data.PieceData[i];
                var pieceData = Generate<PieceData>(rawData.Coordinate);
                pieceData.LastCoordinate = rawData.Coordinate;
                pieceData.Name = rawData.Name;
                pieceData.Team = rawData.Team;
                pieceData.State = rawData.State;
                pieceData.Color = rawData.Color;
                var piece = Generate(m_Piece, pieceData, m_PiecePool);
                mPieces[i] = piece;

                if (rawData.State != PieceState.Dead && TryGetBoard(rawData.Coordinate, out var board))
                    board.SetPiece(piece);
            }
        }

        public void Save()
        {
            var data = new GameData
            {
                GameStep = GameStepPipeline.GameStep,
                CurrentPieceType = mCurrentPieceType,
                PieceTypeCount = mPieceTypeCount,
                BoardRowsCount = mBoardRowsCount
            };

            var boardData = new BoardRawData[mBoards.Length];
            for (var i = 0; i < boardData.Length; i++)
                boardData[i] = BoardRawData.Convert(mBoards[i].Data);

            data.BoardData = boardData;

            var pieceData = new PieceRawData[mPieces.Length];
            for (var i = 0; i < pieceData.Length; i++)
                pieceData[i] = PieceRawData.Convert(mPieces[i].Data);

            data.PieceData = pieceData;

            GameUtility.Save(data);
        }

        public void DoRemoveStepJob()
        {
            DoRemoveTurnJob();
        }

        public void DoMoveStepJob()
        {
            DoMoveTurnJob();
        }

        void DoRemoveTurnJob()
        {
            Save();

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
                        var coordinate = FindBoardHasNoPiece();
                        SetAroundPieceToRemovable(coordinate);
                        mSelectedPiece = null;
                    }
                    break;
            }

            Debug.LogFormat("Start {0} Remove-Turn", mCurrentPieceType);
        }

        void DoMoveTurnJob()
        {
            Save();

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
                {
                    mCurrentPieceType = NextPieceType;
                    OnRemoveStepDone();
                }
                else
                {
                    mCurrentPieceType = NextPieceType;
                    DoRemoveTurnJob();
                }
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
            MovePieceToCoordinates(mSelectedPiece, coordinates, () => { mCurrentPieceType = NextPieceType; DoMoveTurnJob(); });
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