using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Konane.Game
{
    public partial class Piece
    {
        Sequence mMoveAnim = null;
        Queue<MoveTweenUnit> mNextCoordinates = new Queue<MoveTweenUnit>();

        void Awake()
        {
            m_Button.OnDown.AddListener(OnButtonDown);
        }

        void OnDestroy()
        {
            m_Button.OnDown.RemoveAllListeners();
            m_Button.OnUp.RemoveAllListeners();
            m_Button.OnUpAsButton.RemoveAllListeners();

            ClearEvents();
        }

        void DoSetName(string name)
        {
            gameObject.name = name;
        }

        void DoSetTeam(int team)
        {

        }

        void DoSetCooridinate(Coordinate coordinate)
        {
            transform.position = GameUtility.CoordinateToPosition(coordinate);
        }

        void DoSetCooridinateInTween(Coordinate coordinate, Action onDone = null)
        {
            var unit = new MoveTweenUnit(coordinate, onDone);
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

        void OnButtonDown()
        {
            OnDown.Invoke(this);
        }

        class MoveTweenUnit
        {
            public Coordinate Coordinate;
            public Action OnComplete;

            public MoveTweenUnit(Coordinate coordinate, Action onDone)
            {
                Coordinate = coordinate;
                OnComplete = onDone;
            }
        }
    }
}