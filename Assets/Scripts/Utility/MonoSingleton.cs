using UnityEngine;

namespace Konane
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static bool IsExist => Instance != null;
        public static T Instance { get; private set; }

        public static T GetOrCreate(string prefabPath = null)
        {
            if (IsExist)
                return Instance;

            if (string.IsNullOrEmpty(prefabPath))
                return new GameObject(typeof(T).Name).AddComponent<T>();
            else
                return Instantiate(Resources.Load<T>(prefabPath));
        }

        protected virtual bool Immortal => false;

        protected virtual void Awake()
        {
            Instance = this as T;

            if (Immortal)
                DontDestroyOnLoad(this);
        }

        protected virtual void OnDestroy()
        {
            Instance = null;
        }
    }
}