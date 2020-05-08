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

    public class GameLogic : MonoBehaviour
    {
        [SerializeField]
        CheckerManager m_CheckerManager = null;

        public GameStep GameStep { get; private set; }

        void Start()
        {
            GameStep = GameStep.GameBegin;

            m_CheckerManager.OnRemoveStepDone += OnRemoveStepDone;
            m_CheckerManager.SetupCheckers(2, 6);

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
            m_CheckerManager.DoRemoveStepJob();
        }

        void DoMoveStepJob()
        {
            GameStep = GameStep.Move;
            m_CheckerManager.DoMoveStepJob();
        }

        void OnRemoveStepDone()
        {
            NextStep();
        }
    }
}