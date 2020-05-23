using System;
using UnityEngine;

namespace Konane
{
    public class InputManager : MonoSingleton<InputManager>
    {
        public static Action OnEscape = delegate { };

        protected override bool Immortal => true;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnEscape();
            }
        }
    }
}