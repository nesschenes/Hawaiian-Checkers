using System.Collections;
using UnityEngine;

namespace Konane.Game
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
        GameManager m_GameManager = null;

        public GameStep GameStep { get; private set; }

        IEnumerator Start()
        {
            GameStep = GameStep.GameBegin;

            Notify.RefreshScaler.Invoke();

            m_GameManager.Generate();

            yield return null;

            NextStep();
        }

        void OnDestroy()
        {
            m_GameManager.OnRemoveStepDone -= OnRemoveStepDone;
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
                case GameStep.Move:
                    DoGameOverJob();
                    break;
                case GameStep.GameOver:
                    DoBackToLobbyJob();
                    break;
            }
        }

        void DoRemoveStepJob()
        {
            GameStep = GameStep.Remove;
            m_GameManager.OnRemoveStepDone += OnRemoveStepDone;
            m_GameManager.DoRemoveStepJob();
        }

        void DoMoveStepJob()
        {
            GameStep = GameStep.Move;
            m_GameManager.OnMoveStepDone += OnMoveStepDone;
            m_GameManager.DoMoveStepJob();
        }

        void DoGameOverJob()
        { 
            // show game result
        }

        void DoBackToLobbyJob()
        { 
            // load scene
        }

        void OnRemoveStepDone()
        {
            m_GameManager.OnRemoveStepDone -= OnRemoveStepDone;
            NextStep();
        }

        void OnMoveStepDone()
        {
            m_GameManager.OnMoveStepDone -= OnMoveStepDone;
            NextStep();
        }
    }
}