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

        int mCurrentPieceTeam = 1;
        int mPieceTeamCount = 2;
        int mBoardGridCount = 36;
        int mBoardRowsCount = 6;
        Board[] mBoards = null;
        Piece[] mPieces = null;
        Piece mSelectedPiece = null;
    }
}