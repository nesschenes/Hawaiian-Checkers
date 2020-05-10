using Konane.Utility;
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

        void Awake()
        {
            Notify.LoadLobby += DoBackToLobbyJob;
        }

        IEnumerator Start()
        {
            GameStep = GameStep.GameBegin;

            Notify.RefreshScaler();

            m_GameManager.Generate();

            yield return null;

            NextStep();
        }

        void OnDestroy()
        {
            m_GameManager.OnRemoveStepDone -= OnRemoveStepDone;
            m_GameManager.OnMoveStepDone -= OnMoveStepDone;
            Notify.LoadLobby -= DoBackToLobbyJob;
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
            GameStep = GameStep.GameOver;
            Notify.GameResult(m_GameManager.Winner);
        }

        void DoBackToLobbyJob()
        {
            SceneUtility.LoadLobbyScene();
            SceneUtility.LoadLobbyUIScene();
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