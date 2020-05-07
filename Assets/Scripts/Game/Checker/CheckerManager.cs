using System;
using UnityEngine;

namespace Hawaiian.Game
{
    public class CheckerManager : MonoBehaviour
    {
        [SerializeField]
        Checker m_Checker = null;
        [SerializeField]
        Transform m_CheckerPool = null;
        [SerializeField]
        Color[] m_CheckerColor = null;

        public Action OnRemoveStepDone = null;

        bool IsThisRoundOver => NextTurn < mCurrentTurn;
        int NextTurn => mCurrentTurn == mTotalTurnPerRound ? 1 : mCurrentTurn + 1;
        int mCurrentTurn = 1;
        int mTotalTurnPerRound = 2;
        BoardSize mBoardSize = BoardSize.Six;
        Checker[] mCheckers = null;
        Checker mSelectedChecker = null;
        int mLastRemovedCheckerIndex = 0;

        public void SetupCheckers(int turnPerRound, BoardSize size)
        {
            mTotalTurnPerRound = turnPerRound;
            mBoardSize = size;
            var count = (int)size;
            mCheckers = new Checker[count * count];

            var halfWidth = count / 2f;
            var startPos = new Vector2(halfWidth * (-1) + 0.5f, halfWidth - 0.5f); // left-top
            for (var i = 0; i < count; i++) // rows
            {
                for (var j = 0; j < count; j++) // columns
                {
                    var team = (i + j) % 2;
                    var data = new CheckerData
                    {
                        Name = $"Checker {j + 1} - {i + 1}",
                        Index = i * count + j,
                        Team = team,
                        Position = startPos + new Vector2(j, -i),
                        Color = m_CheckerColor[team],
                    };

                    var checker = SpawnChecker(data);
                    mCheckers[i * count + j] = checker;
                }
            }
        }

        public void DoRemoveStepJob()
        {
            mCurrentTurn = 0;
            DoNextRemoveTurnJob();
        }

        void DoNextRemoveTurnJob()
        {
            ++mCurrentTurn;
            switch (mCurrentTurn)
            {
                case 1:
                    {
                        var totalCount = mCheckers.Length;
                        var rowCount = (int)mBoardSize;
                        SetAsRemovable(mCheckers[totalCount / 2 + rowCount / 2 - rowCount - 1]);
                        SetAsRemovable(mCheckers[totalCount / 2 + rowCount / 2]);
                        SetAsRemovable(mCheckers[0]);
                        SetAsRemovable(mCheckers[mCheckers.Length - 1]);
                    }
                    break;
                case 2:
                    {
                        var rowCount = (int)mBoardSize;
                        if (mLastRemovedCheckerIndex == 0)
                        {
                            SetAsRemovable(mCheckers[mLastRemovedCheckerIndex + 1]);
                            SetAsRemovable(mCheckers[mLastRemovedCheckerIndex + rowCount]);
                        }
                        else if (mLastRemovedCheckerIndex == mCheckers.Length - 1)
                        {
                            SetAsRemovable(mCheckers[mLastRemovedCheckerIndex - 1]);
                            SetAsRemovable(mCheckers[mLastRemovedCheckerIndex - rowCount]);
                        }
                        else
                        {
                            SetAsRemovable(mCheckers[mLastRemovedCheckerIndex - 1]);
                            SetAsRemovable(mCheckers[mLastRemovedCheckerIndex + 1]);
                            SetAsRemovable(mCheckers[mLastRemovedCheckerIndex - rowCount]);
                            SetAsRemovable(mCheckers[mLastRemovedCheckerIndex + rowCount]);
                        }
                    }
                    break;
            }
        }

        Checker SpawnChecker(CheckerData data)
        {
            var checker = Instantiate(m_Checker, m_CheckerPool);
            checker.Init(data);

            return checker;
        }

        void SetAsNothingToDo(Checker[] checkers)
        {
            foreach (var checker in checkers)
                checker.SetAsNothingToDo();
        }

        void SetAsRemovable(Checker checker)
        {
            checker.OnUpAsButton.AddListener(OnRemovableSelected);
            checker.SetAsInteractable();
        }

        void OnRemovableSelected(Checker checker)
        {
            if (mSelectedChecker == checker)
            {
                if (mSelectedChecker != null)
                {
                    mSelectedChecker.Dispose();
                    mLastRemovedCheckerIndex = mSelectedChecker.Data.Index;

                    if (IsThisRoundOver)
                    {
                        SetAsNothingToDo(mCheckers);
                        OnRemoveStepDone.Invoke();
                    }
                    else
                    {
                        SetAsNothingToDo(mCheckers);
                        DoNextRemoveTurnJob();
                    }
                }
            }
            else
            {
                mSelectedChecker = checker;
                mSelectedChecker?.SetAsWaitToRemove();
            }
        }

        void OnUp(Checker checker)
        {

        }
    }
}