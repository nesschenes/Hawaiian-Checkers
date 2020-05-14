using System;
using UnityEngine;

namespace Konane.Game
{
    public partial class Piece : CoordinateMono<Piece, PieceData>, ICoordinateEntity<PieceData>
    {
        [SerializeField]
        SpriteRenderer m_HighlightIcon = null;
        [SerializeField]
        SpriteRenderer m_SelectedIcon = null;

        public PieceData Data { get; private set; }
        public string Name { get => Data.Name; private set => Data.Name = value; } 
        public int Team { get => Data.Team; private set => Data.Team = value; }
        public PieceState State { get => Data.State; private set => Data.State = value; }
        public Coordinate Coordinate { get => Data.Coordinate; private set { Data.LastCoordinate = Data.Coordinate; Data.Coordinate = value; } }
        public Coordinate LastCoordinate { get => Data.LastCoordinate; }
        public Color Color { get => Data.Color; private set => Data.Color = value; }

        public void Init(PieceData data)
        {
            Data = data;

            DoSetName(data.Name);
            DoSetTeam(data.Team);
            DoSetState(data.State);
            DoSetCooridinate(data.Coordinate);
            DoSetColor(data.Color);
        }

        public void ClearEvents()
        {
            OnDown.RemoveAllListeners();
        }

        public void SetAsNothingToDo()
        {
            State = PieceState.None;
            ToggleIcon(true);
            ToggleHighlight(false);
            ToggleSelected(false);
            SetInteractable(false);
        }

        public void SetAsRemovable()
        {
            State = PieceState.Removable;
            ToggleIcon(true);
            ToggleHighlight(true);
            ToggleSelected(false);
            SetInteractable(true);
        }

        public void SetAsWaitToRemove()
        {
            State = PieceState.WaitToRemove;
            ToggleIcon(true);
            ToggleHighlight(false);
            ToggleSelected(true);
            SetInteractable(true);
        }

        public void SetAsMovable()
        {
            State = PieceState.Movable;
            ToggleIcon(true);
            ToggleHighlight(true);
            ToggleSelected(false);
            SetInteractable(true);
        }

        public void SetAsWaitToMove()
        {
            State = PieceState.WaitToMove;
            ToggleIcon(true);
            ToggleHighlight(false);
            ToggleSelected(true);
            SetInteractable(true);
        }

        public void SetAsDead()
        {
            State = PieceState.Dead;
            ToggleIcon(false);
            ToggleHighlight(false);
            ToggleSelected(false);
            SetInteractable(false);

            OnDespawn.Invoke(this);
        }

        public void SetName(string name)
        {
            Name = name;
            DoSetName(name);
        }

        public void SetTeam(int team)
        {
            Team = team;
            DoSetTeam(team);
        }

        public void SetCoordinate(Coordinate coordinate)
        {
            Coordinate = coordinate;
            DoSetCooridinate(coordinate);
        }

        public void SetCoordinateInTween(Coordinate coordinate, Action onComplete = null)
        {
            Coordinate = coordinate;
            DoSetCooridinateInTween(coordinate, onComplete);
        }

        public void SetColor(Color color)
        {
            Color = color;
            DoSetColor(color);
        }
    }
}