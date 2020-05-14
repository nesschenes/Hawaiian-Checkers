using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Konane.Game
{
    public partial class Piece
    {
        Sequence mMoveAnim = null;
        Queue<ICoordinateTween> mNextCoordinates = new Queue<ICoordinateTween>();

        void DoSetName(string name)
        {
            gameObject.name = name;
        }

        void DoSetTeam(int team)
        {

        }

        void DoSetState(PieceState state)
        {
            switch (state)
            {
                case PieceState.None:
                    SetAsNothingToDo();
                    break;
                case PieceState.Removable:
                    SetAsRemovable();
                    break;
                case PieceState.WaitToRemove:
                    SetAsWaitToRemove();
                    break;
                case PieceState.Movable:
                    SetAsMovable();
                    break;
                case PieceState.WaitToMove:
                    SetAsWaitToMove();
                    break;
                case PieceState.Dead:
                    SetAsDead();
                    break;
                default:
                    Debug.LogErrorFormat("Undefined PieceState : {0}", state);
                    break;
            }
        }

        void DoSetCooridinate(Coordinate coordinate)
        {
            transform.position = GameUtility.CoordinateToPosition(coordinate);
        }

        void DoSetCooridinateInTween(Coordinate coordinate, Action onComplete = null)
        {
            var unit = new CoordniateTween(coordinate, onComplete);
            mNextCoordinates.Enqueue(unit);
            DoNextCooridinateInTween();
        }

        void DoNextCooridinateInTween()
        {
            if (mNextCoordinates.Count == 0)
                return;

            if (mMoveAnim != null && mMoveAnim.IsActive() && mMoveAnim.IsPlaying())
                return;

            var unit = mNextCoordinates.Dequeue();
            var position = GameUtility.CoordinateToPosition(unit.Coordinate);
            mMoveAnim = DOTween.Sequence()
                               .Append(transform.DOMove(position, 0.5f))
                               .Join(transform.DOPunchScale(new Vector3(1.2f, 1.2f), 0.5f, 1))
                               .OnStart(() => m_Icon.sortingOrder = 999)
                               .OnComplete(() =>
                               {
                                   m_Icon.sortingOrder = 1;
                                   unit.OnComplete?.Invoke();
                                   DoNextCooridinateInTween();
                               })
                               .Play();
        }

        void DoSetColor(Color color)
        {
            m_Icon.color = color;
        }

        void ToggleIcon(bool active)
        {
            m_Icon.enabled = active;
        }

        void ToggleHighlight(bool active)
        {
            m_HighlightIcon.enabled = active;
        }

        void ToggleSelected(bool active)
        {
            m_SelectedIcon.enabled = active;
        }

        protected override void OnButtonDown()
        {
            OnDown?.Invoke(this);
        }
    }
}