using Konane.Renderer;
using UnityEngine;
using UnityEngine.Events;

namespace Konane.Game
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

        public bool HasPiece => Piece != null;
        public Piece Piece { get; private set; }
        public BoardData Data { get; private set; }
        public string Name { get => Data.Name; private set => Data.Name = value; }
        public BoardState State { get => Data.State; private set => Data.State = value; }
        public Coordinate Coordinate { get => Data.Coordinate; private set => Data.Coordinate = value; }
        public Color Color { get => Data.Color; private set => Data.Color = value; }

        public BoardEvent OnDown = new BoardEvent();
        public BoardEvent OnDespawn = new BoardEvent();

        public class BoardEvent : UnityEvent<Board> { }

        public void Init(BoardData data)
        {
            Data = data;

            DoSetName(data.Name);
            DoSetState(data.State);
            DoSetColor(data.Color);
            DoSetCoordinate(data.Coordinate);

            SetRandomUV();
        }

        public void ClearInputEvents()
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

        public void SetState(BoardState state)
        {
            if (State == state)
                return;

            State = state;
            DoSetState(State);
        }

        public void SetInteractable(bool enable)
        {
            if (enable)
                m_Button.Active();
            else
                m_Button.Deactive();
        }
    }
}