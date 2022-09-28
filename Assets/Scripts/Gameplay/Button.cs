using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexGame.Gameplay
{
    public class Button : HexGridElement, IButton, IAttachedToPlatform, IResetState
    {
        [SerializeField] private List<ButtonConnection> _buttonConnections = new();

        private Platform _parentPlatform;
        
        public event Action Pressed;
        public event Action Released;
        public event Action Reset;
        
        public bool IsPressed { get; private set; }

        public void AttachToPlatform(Platform parentPlatform)
        {
            _parentPlatform = parentPlatform;
            transform.parent = _parentPlatform.transform;
        }

        public void Press()
        {
            if (IsPressed) return;
            IsPressed = true;
            ActivateConnections();
            Pressed?.Invoke();
        }

        public void Release()
        {
            if (!IsPressed) return;
            IsPressed = false;
            DeactivateConnections();
            Released?.Invoke();
        }
        
        public void ResetState()
        {
            IsPressed = false;
            DeactivateConnections();
            Reset?.Invoke();
        }

        private void ActivateConnections()
        {
            foreach (var connection in _buttonConnections)
                connection.Activate();
        }
        
        private void DeactivateConnections()
        {
            foreach (var connection in _buttonConnections)
                connection.Deactivate();
        }
    }
}