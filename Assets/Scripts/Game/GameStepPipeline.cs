using Konane.Utility;
using UnityEngine;

namespace Konane.Game
{
    public class GameStepPipeline : MonoBehaviour
    {
        [SerializeField]
        GameManager m_GameManager = null;

        public static GameStep GameStep { get; private set; }

        void Awake()
        {
            Notify.LoadLobby += DoBackToLobbyJob;
            InputManager.OnEscape += OnEscape;
        }

        void Start()
        {
            Notify.RefreshScaler();

            GameStep = GameStep.GameBegin;

            DoStepJob();
        }

        void OnDestroy()
        {
            m_GameManager.OnRemoveStepDone -= OnRemoveStepDone;
            m_GameManager.OnMoveStepDone -= OnMoveStepDone;
            Notify.LoadLobby -= DoBackToLobbyJob;
            InputManager.OnEscape -= OnEscape;
        }

        void DoStepJob()
        {
            switch (GameStep)
            {
                case GameStep.GameBegin:
                    DoGameBeginStepJob();
                    break;
                case GameStep.Remove:
                    DoRemoveStepJob();
                    break;
                case GameStep.Move:
                    DoMoveStepJob();
                    break;
                case GameStep.GameOver:
                    DoGameOverJob();
                    break;
            }
        }

        void DoGameBeginStepJob()
        {
            if (GameData.LastData != null)
            {
                m_GameManager.Resume(GameData.LastData);

                GameStep = GameData.LastData.GameStep;
            }
            else
            {
                m_GameManager.Generate();

                GameStep = GameStep.Remove;
            }

            DoStepJob();
        }

        void DoRemoveStepJob()
        {
            m_GameManager.OnRemoveStepDone += OnRemoveStepDone;
            m_GameManager.DoRemoveStepJob();
        }

        void DoMoveStepJob()
        {
            m_GameManager.OnMoveStepDone += OnMoveStepDone;
            m_GameManager.DoMoveStepJob();
        }

        void DoGameOverJob()
        {
            GameUtility.Clear();
            Notify.GameResult(m_GameManager.Winner);
        }

        void DoBackToLobbyJob()
        {
            SceneUtility.LoadMenuScene();
            SceneUtility.LoadMenuUIScene();
        }

        void OnRemoveStepDone()
        {
            m_GameManager.OnRemoveStepDone -= OnRemoveStepDone;

            GameStep = GameStep.Move;
            DoStepJob();
        }

        void OnMoveStepDone()
        {
            m_GameManager.OnMoveStepDone -= OnMoveStepDone;

            GameStep = GameStep.GameOver;
            DoStepJob();
        }

        void OnEscape()
        {
            switch (GameStep)
            {
                default:
                    m_GameManager.Save();
                    DoBackToLobbyJob();
                    break;
                case GameStep.GameOver:
                    GameUtility.Clear();
                    DoBackToLobbyJob();
                    break;
            }
        }
    }
}