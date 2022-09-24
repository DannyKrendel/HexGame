using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HexGame.Input
{
    public class InputManager
    {
        private readonly GameInput _gameInput;

        public Vector2 PointerPosition => _gameInput.UI.Point.ReadValue<Vector2>();
        public event Action<Vector2> Click;
        
        public InputManager(GameInput gameInput)
        {
            _gameInput = gameInput;
            _gameInput.UI.Click.performed += OnClick;
        }

        private void OnClick(InputAction.CallbackContext context)
        {
            if (context.ReadValue<float>() == 0) return;
            Click?.Invoke(PointerPosition);
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