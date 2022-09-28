using System;
using UnityEngine;

namespace HexGame.Gameplay
{
    public class Door : HexGridElement, IDoor, IAttachedToPlatform
    {
        private Platform _parentPlatform;
        
        public event Action Opened;
        public event Action Closed;
        public event Action Reset;
        
        public bool IsOpen { get; private set; }

        public void AttachToPlatform(Platform parentPlatform)
        {
            _parentPlatform = parentPlatform;
            transform.parent = _parentPlatform.transform;
        }
        
        public void Open()
        {
            if (IsOpen) return;
            IsOpen = true;
            Opened?.Invoke();
        }

        public void Close()
        {
            if (!IsOpen) return;
            IsOpen = false;
            Closed?.Invoke();
        }
        
        public void ResetState()
        {
            IsOpen = false;
            Reset?.Invoke();
        }
    }
}