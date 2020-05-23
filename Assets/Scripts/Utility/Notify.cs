using System;

namespace Konane
{
    public static class Notify
    {
        /// <summary> notify resume the last game </summary>
        public static Action ResumeGame = delegate { };

        /// <summary> notify load lobby scene to choose the game </summary>
        public static Action LoadLobby = delegate { };

        /// <summary> notify load game scene to start the game </summary>
        public static Action LoadGame = delegate { };

        /// <summary> notify camera or sprite which needs to refresh with scaler </summary>
        public static Action RefreshScaler = delegate { };

        /// <summary> which piece type wins the game </summary>
        public static Action<int> GameResult = delegate { };
    }
}