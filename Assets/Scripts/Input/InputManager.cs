using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HexGame.Input
{
    public class InputManager
    {
        private readonly GameInput _gameInput;

        public Vector2 PointerPosition => _gameInput.UI.Point.ReadValue<Vector2>();
        public event Action<Vector2> TapStarted;
        public event Action<Vector2> TapEnded;
        public event Action<Vector2> LongTap;
        
        public InputManager(GameInput gameInput)
        {
            _gameInput = gameInput;
            _gameInput.UI.Click.performed += OnTap;
            _gameInput.UI.LongClick.performed += OnLongTap;
        }

        private void OnTap(InputAction.CallbackContext context)
        {
            if (context.ReadValue<float>() == 1)
                TapStarted?.Invoke(PointerPosition);
            else
                TapEnded?.Invoke(PointerPosition);
        }

        private void OnLongTap(InputAction.CallbackContext context)
        {
            if (context.ReadValue<float>() == 0) return;
            LongTap?.Invoke(PointerPosition);
        }

        public void Enable()
        {
            _gameInput.Enable();
        }

        public void Disable()
        {
            _gameInput.Disable();
        }
    }
}