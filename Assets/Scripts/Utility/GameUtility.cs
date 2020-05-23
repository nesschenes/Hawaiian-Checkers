using System.IO;
using UnityEngine;

namespace Konane.Game
{
    public static class GameUtility
    {
        public static string GameDataPath => $"{Application.dataPath}/GameData.txt";

        public static Vector2 CoordinateToPosition(Coordinate coordinate)
        {
            if (!GameManager.IsExist)
            {
                Debug.LogErrorFormat("{0} isn't exist !", nameof(GameManager));
                return Vector2.zero;
            }

            return GameManager.Instance.BoardStartPosition + new Vector2(coordinate.x, coordinate.y);
        }

        public static void Save(GameData data)
        {
            var json = JsonUtility.ToJson(data);
            var fileStream = new FileStream(GameDataPath, FileMode.Create);
            var writeStream = new StreamWriter(fileStream);
            writeStream.Write(json);
            writeStream.Close();
            fileStream.Close();

            Debug.Log("Game Saved");
        }

        public static GameData Load()
        {
            if (!File.Exists(GameDataPath))
                return null;

            var fileStream = new FileStream(GameDataPath, FileMode.Open);
            var readStream = new StreamReader(fileStream);
            var json = readStream.ReadToEnd();
            readStream.Close();
            fileStream.Close();

            var data = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game Loaded");

            return data;
        }

        public static void Clear()
        {
            if (File.Exists(GameDataPath))
                File.Delete(GameDataPath);

            GameData.LastData = null;

            Debug.Log("Game Data Clear");
        }
    }
}