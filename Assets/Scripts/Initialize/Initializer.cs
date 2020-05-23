using Konane.Utility;
using UnityEngine;

namespace Konane
{
    public class Initializer : MonoBehaviour
    {
        void Awake()
        {
            Setup();
        }

        void Start()
        {
            SceneUtility.LoadLobbyScene();
            SceneUtility.LoadLobbyUIScene();
        }

        void Setup()
        {
            InputManager.GetOrCreate();
            Dialog.GetOrCreate("Dialog");
        }
    }
}