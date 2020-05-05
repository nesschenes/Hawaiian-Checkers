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

        BoardSize mBoardSize = BoardSize.Six;
        Checker[] mCheckers = null;

        public void SetupCheckers(BoardSize size)
        {
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
                        Name = string.Format("Checker {0} - {1}", j + 1, i + 1),
                        Index = i,
                        Team = team,
                        Position = startPos + new Vector2(j, -i),
                        Color = m_CheckerColor[team],
                    };

                    var checker = SpawnChecker(data);
                    mCheckers[i * count + j] = checker;
                }
            }
        }

        public void BeginRemoveStep()
        {
            var totalCount = mCheckers.Length;
            var rowCount = (int)mBoardSize;
            var removable1 = totalCount / 2 + rowCount / 2 - rowCount - 1;
            var removable2 = totalCount / 2 + rowCount / 2;
            mCheckers[removable1].SetRemovable();
            mCheckers[removable2].SetRemovable();
            mCheckers[0].SetRemovable();
            mCheckers[mCheckers.Length - 1].SetRemovable();
        }

        Checker SpawnChecker(CheckerData data)
        {
            var checker = Instantiate(m_Checker, m_CheckerPool);
            checker.Init(data);

            return checker;
        }
    }
}