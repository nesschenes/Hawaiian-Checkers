using UnityEngine;

namespace Hawaiian.Game
{
    public class GameLogic : MonoBehaviour
    {
        [SerializeField]
        CheckerManager m_CheckerManager = null;

        void Start()
        {
            m_CheckerManager.SetupCheckers(BoardSize.Six);
            BeginRemoveStep();
        }

        void BeginRemoveStep()
        {
            m_CheckerManager.BeginRemoveStep();
        }

        void BeginMoveStep()
        { 

        }
    }
}