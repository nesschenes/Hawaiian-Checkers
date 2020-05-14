using UnityEngine;

namespace Konane.Game
{
    public partial class Board : CoordinateMono<Board, BoardData>, ICoordinateEntity<BoardData>
    {
        [SerializeField]
        SpriteRenderer m_OccupiableIcon = null;

        public bool HasPiece => Piece != null;
        public Piece Piece { get; private set; }
        public BoardData Data { get; private set; }
        public string Name { get => Data.Name; private set => Data.Name = value; }
        public BoardState State { get => Data.State; private set => Data.State = value; }
        public Coordinate Coordinate { get => Data.Coordinate; private set => Data.Coordinate = value; }
        public Color Color { get => Data.Color; private set => Data.Color = value; }

        public void Init(BoardData data)
        {
            Data = data;

            DoSetName(data.Name);
            DoSetState(data.State);
            DoSetCoordinate(data.Coordinate);
            DoSetColor(data.Color);

            SetRandomUV();
        }

        public void ClearEvents()
        {
            OnDown.RemoveAllListeners();
        }

        public void SetName(string name)
        {
            Name = name;
            DoSetName(name);
        }

        public void SetPiece(Piece piece)
        {
            if (Piece == piece)
                return;

            if (Piece != null)
                Piece.OnDespawn.RemoveListener(OnPieceDespawn);

            Piece = piece;

            if (Piece != null)
                Piece.OnDespawn.AddListener(OnPieceDespawn);
        }

        public void SetAsNone()
        {
            State = BoardState.None;
            ToggleOccupiable(false);
            SetInteractable(false);
        }

        public void SetAsOccupiable()
        {
            State = BoardState.Occupiable;
            ToggleOccupiable(true);
            SetInteractable(true);
        }
    }
}