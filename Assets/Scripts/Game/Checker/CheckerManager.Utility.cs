namespace Hawaiian.Game
{
    public partial class CheckerManager
    {
        Checker SpawnChecker(CheckerData data)
        {
            var checker = Instantiate(m_Checker, m_CheckerPool);
            checker.Init(data);
            return checker;
        }

        bool TryGetChecker(Coordinate coordinate, out Checker result)
        {
            if (!InBoard(coordinate))
            {
                result = null;
                return false;
            }

            foreach (var checker in mCheckers)
            {
                if (checker.Data.Coordinate == coordinate)
                {
                    result = checker;
                    return true;
                }
            }

            result = null;
            return false;
        }

        void SetAsNothingToDo(Checker[] checkers)
        {
            foreach (var checker in checkers)
                if (checker.State != CheckerState.Dead)
                    checker.SetAsNothingToDo();
        }

        void SetAroundAsRemovable(Coordinate coordinate)
        {
            SetAsRemovable(coordinate + Coordinate.Top);
            SetAsRemovable(coordinate + Coordinate.Down);
            SetAsRemovable(coordinate + Coordinate.Left);
            SetAsRemovable(coordinate + Coordinate.Right);
        }

        void SetAsRemovable(Coordinate coordinate)
        {
            if (!TryGetChecker(coordinate, out var checker))
                return;

            checker.OnUpAsButton.AddListener(OnRemovableSelected);
            checker.SetAsRemovable();
        }

        bool InBoard(Coordinate coordinate)
        {
            return InBoard(coordinate.X, coordinate.Y);
        }

        bool InBoard(int x, int y)
        {
            if (x < 0)
                return false;
            if (x >= mBoardRowsCount)
                return false;
            if (y < 0)
                return false;
            if (y >= mBoardRowsCount)
                return false;

            return true;
        }

        bool IsEmpty(Coordinate coordinate)
        {
            foreach (var checker in mCheckers)
            {
                if (checker.Data.Coordinate != coordinate)
                    continue;

                if (checker.State == CheckerState.Dead)
                    continue;

                return false;
            }

            return true;
        }
    }
}