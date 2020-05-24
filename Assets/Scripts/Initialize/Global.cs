using UnityEngine;

namespace Konane
{
    public static class Global
    {
        public static string Version { get; private set; }

        static Global()
        {
            Version = Application.version;
        }
    }
}