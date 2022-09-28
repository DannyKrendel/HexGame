using System;
using UnityEngine;

namespace HexGame.Gameplay
{
    public class Button : HexGridElement, IButton
    {
        private Platform _parentPlatform;
        
        public event Action Pressed;
        public event Action Released;
        
        public bool IsPressed { get; private set; }

        public void SetParentPlatform(Platform parentPlatform)
        {
            _parentPlatform = parentPlatform;
            transform.parent = _parentPlatform.transform;
        }

        public void Press()
        {
            if (IsPressed) return;
            IsPressed = true;
            Pressed?.Invoke();
            Debug.Log("Pressed");
        }

        public void Release()
        {
            if (!IsPressed) return;
            IsPressed = false;
            Released?.Invoke();
            Debug.Log("Released");
        }
    }
}