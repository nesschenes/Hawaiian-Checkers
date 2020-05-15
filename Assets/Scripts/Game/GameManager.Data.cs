using System.Collections.Generic;
using UnityEngine;

namespace Konane.Game
{
    public partial class GameManager
    {
        [SerializeField]
        Board m_Board = null;
        [SerializeField]
        Piece m_Piece = null;
        [SerializeField]
        Transform m_BoardPool = null;
        [SerializeField]
        Transform m_PiecePool = null;
        [SerializeField]
        Color[] m_BoardColor = null;
        [SerializeField]
        Color[] m_PieceColor = null;

        int mCurrentPieceType = 1;
        int mPieceTypeCount = 2;
        int mBoardGridCount = 36;
        int mBoardRowsCount = 6;
        Board[] mBoards = null;
        Piece[] mPieces = null;
        Piece mSelectedPiece = null;

        /// <summary> mapping coordinate(if the piece move to here) to the paths of coordinate </summary>
        Dictionary<Coordinate, Queue<Coordinate>> mOccupiablePathDict = new Dictionary<Coordinate, Queue<Coordinate>>(8);
        /// <summary> mapping coordinate(if the piece move to here) to the coordinate be eaten </summary>
        Dictionary<Coordinate, Piece> mEatablePieceDict = new Dictionary<Coordinate, Piece>();
    }
}