using System;
using UnityEngine;

namespace Hawaiian.Game
{
    public partial class CheckerManager : MonoBehaviour
    {
        public Action OnRemoveStepDone = null;

        bool IsThisRoundOver => NextTurn < mCurrentTurn;
        int NextTurn => mCurrentTurn == mTurnCountPerRound ? 1 : mCurrentTurn + 1;

        public void SetupCheckers(int turnCountPerRound, int rowsCount)
        {
            mTurnCountPerRound = turnCountPerRound;
            mBoardRowsCount = rowsCount;
            mBoardGridCount = rowsCount * rowsCount;
            mCheckers = new Checker[mBoardGridCount];

            var halfWidth = rowsCount / 2f;
            var startPos = new Vector2(halfWidth * (-1) + 0.5f, -halfWidth + 0.5f); // left-top
            for (var i = 0; i < rowsCount; i++) // rows
            {
                for (var j = 0; j < rowsCount; j++) // columns
                {
                    var team = (i + j + 1) % 2;
                    var data = new CheckerData
                    {
                        Name = $"Checker {j + 1} - {i + 1}",
                        Coordinate = new Coordinate(j, i),
                        Team = team,
                        Position = startPos + new Vector2(j, i),
                        Color = m_CheckerColor[team],
                    };

                    var checker = SpawnChecker(data);
                    mCheckers[i * rowsCount + j] = checker;
                }
            }
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
                    SetAsRemovable(new Coordinate(0, mBoardRowsCount - 1));
                    SetAsRemovable(new Coordinate(mBoardRowsCount - 1, 0));
                    SetAsRemovable(new Coordinate(mBoardRowsCount / 2 - 1, mBoardRowsCount / 2));
                    SetAsRemovable(new Coordinate(mBoardRowsCount / 2, mBoardRowsCount / 2 - 1));
                }
                    break;
                case 2:
                {
                    SetAroundAsRemovable(mLastEmptyCoordinate);
                }
                    break;
            }
        }

        void DoNextMoveTurnJob()
        {
            ++mCurrentTurn;
            foreach (var checker in mCheckers)
            {
                if (mCurrentTurn != checker.Data.Team + 1)
                    continue;

                FindMovableCoordinate(checker.Data.Coordinate);
            }
        }

        void FindMovableCoordinate(Coordinate coordinate)
        {
            var topCoordinate = coordinate + Coordinate.Top * 2;
            if (InBoard(topCoordinate) && IsEmpty(topCoordinate) && !IsEmpty(coordinate + Coordinate.Top))
                Debug.LogError(topCoordinate);

            var downCoordinate = coordinate + Coordinate.Down * 2;
            if (InBoard(downCoordinate) && IsEmpty(downCoordinate) && !IsEmpty(coordinate + Coordinate.Down))
                Debug.LogError(downCoordinate);

            var leftCoordinate = coordinate + Coordinate.Left * 2;
            if (InBoard(leftCoordinate) && IsEmpty(leftCoordinate) && !IsEmpty(coordinate + Coordinate.Left))
                Debug.LogError(leftCoordinate);

            var rightCoordinate = coordinate + Coordinate.Right * 2;
            if (InBoard(rightCoordinate) && IsEmpty(rightCoordinate) && !IsEmpty(coordinate + Coordinate.Right))
                Debug.LogError(rightCoordinate);
        }

        void OnRemovableSelected(Checker checker)
        {
            if (mSelectedChecker == checker)
            {
                if (mSelectedChecker != null)
                {
                    mSelectedChecker.SetAsDead();
                    mLastEmptyCoordinate = mSelectedChecker.Data.Coordinate;

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
    }
}