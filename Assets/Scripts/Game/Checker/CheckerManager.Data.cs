using UnityEngine;

namespace Hawaiian.Game
{
    public partial class CheckerManager
    {
        [SerializeField]
        Checker m_Checker = null;
        [SerializeField]
        Transform m_CheckerPool = null;
        [SerializeField]
        Color[] m_CheckerColor = null;

        int mCurrentTurn = 1;
        int mTurnCountPerRound = 2;
        int mBoardGridCount = 36;
        int mBoardRowsCount = 6;
        Checker[] mCheckers = null;
        Checker mSelectedChecker = null;
        Coordinate mLastEmptyCoordinate = new Coordinate();
    }
}