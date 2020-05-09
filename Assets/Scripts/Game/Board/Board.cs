using Hawaiian.Sprite;
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Game
{
    public enum BoardState
    { 
        None,
        Occupiable,
    }

    public partial class Board : MonoBehaviour
    {
        [SerializeField]
        Button m_Button = null;
        [SerializeField]
        SpriteRenderer m_Icon = null;
        [SerializeField]
        SpriteRenderer m_OccupiableIcon = null;

        public Piece Piece { get; private set; }
        public BoardData Data { get; private set; }
        public string Name { get => Data.Name; private set => Data.Name = value; }
        public BoardState State { get => Data.State; private set => Data.State = value; }
        public Coordinate Coordinate { get => Data.Coordinate; private set => Data.Coordinate = value; }
        public Color Color { get => Data.Color; private set => Data.Color = value; }

        public bool HasPiece => Piece != null;

        public BoardEvent OnDespawn = new BoardEvent();
        public BoardEvent OnDown = new BoardEvent();
        public BoardEvent OnUp = new BoardEvent();
        public BoardEvent OnUpAsButton = new BoardEvent();

        public class BoardEvent : UnityEvent<Board> { }

        public void Init(BoardData data)
        {
            Data = data;

            DoSetState(data.State);
            DoSetColor(data.Color);

            var position = GameManager.ConvertToPosition(data.Coordinate);
            DoSetPosition(position);

            SetRandomUV();
        }

        public void ClearEvents()
        {
            OnDown.RemoveAllListeners();
            OnUp.RemoveAllListeners();
            OnUpAsButton.RemoveAllListeners();
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

        public void SetState(BoardState state)
        {
            if (State == state)
                return;

            State = state;
            DoSetState(State);
        }
    }
}