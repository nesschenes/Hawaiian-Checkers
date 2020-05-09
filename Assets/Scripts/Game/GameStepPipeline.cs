using System.Collections;
using UnityEngine;

namespace Hawaiian.Game
{
    public enum GameStep
    {
        GameBegin,
        Remove,
        Move,
        GameOver,
    }

    public class GameStepPipeline : MonoBehaviour
    {
        [SerializeField]
        GameManager m_CheckerManager = null;

        public GameStep GameStep { get; private set; }

        IEnumerator Start()
        {
            GameStep = GameStep.GameBegin;

            m_CheckerManager.Setup(2, 6);

            yield return null;

            NextStep();
        }

        void OnDestroy()
        {
            m_CheckerManager.OnRemoveStepDone -= OnRemoveStepDone;
        }

        void NextStep()
        {
            switch (GameStep)
            {
                case GameStep.GameBegin:
                    DoRemoveStepJob();
                    break;
                case GameStep.Remove:
                    DoMoveStepJob();
                    break;
            }
        }

        void DoRemoveStepJob()
        {
            GameStep = GameStep.Remove;
            m_CheckerManager.OnRemoveStepDone += OnRemoveStepDone;
            m_CheckerManager.DoRemoveStepJob();
        }

        void DoMoveStepJob()
        {
            GameStep = GameStep.Move;
            m_CheckerManager.DoMoveStepJob();
        }

        void OnRemoveStepDone()
        {
            m_CheckerManager.OnRemoveStepDone -= OnRemoveStepDone;
            NextStep();
        }
    }
}