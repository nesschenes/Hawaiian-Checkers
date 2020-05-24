using UnityEngine;
using UnityEngine.UI;

namespace Konane.Lobby
{
    public class VersionUI : MonoBehaviour
    {
        [SerializeField]
        Text m_Version = null;

        void Start()
        {
            m_Version.text = $"ver. {Global.Version}";
        }
    }
}