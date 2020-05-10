using Konane.Utility;
using UnityEngine;

namespace Konane
{
    public class Initializer : MonoBehaviour
    {
        void Start()
        {
            SceneUtility.LoadLobbyScene();
            SceneUtility.LoadLobbyUIScene();
        }
    }
}