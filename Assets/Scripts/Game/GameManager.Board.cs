namespace Konane.Game
{
    public partial class GameManager
    {
        bool TryGetBoard(Coordinate coordinate, out Board result)
        {
            if (!IsValid(coordinate))
            {
                result = null;
                return false;
            }

            foreach (var board in mBoards)
            {
                if (board.Coordinate != coordinate) 
                    continue;

                result = board;
                return true;
            }

            result = null;
            return false;
        }

        void SetBoardToNone(Board[] boards)
        {
            foreach (var board in boards)
            {
                board.ClearEvents();
                board.SetAsNone();
            }
        }

        void SetBoardToOccupiable(Coordinate coordinate)
        {
            if (!TryGetBoard(coordinate, out var board))
                return;

            board.OnDown.AddListener(OnOccupiableBoardSelected);
            board.SetAsOccupiable();
        }

        bool HasPiece(Coordinate coordinate)
        {
            foreach (var board in mBoards)
                if (board.Coordinate == coordinate)
                    return board.HasPiece;

            return false;
        }
    }
}