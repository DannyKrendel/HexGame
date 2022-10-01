using System;
using UnityEngine;

namespace HexGame.Gameplay
{
    public class Door : HexGridElement, IDoor, IAttachedToPlatform, IResetState
    {
        public event Action Opened;
        public event Action Closed;
        public event Action Reset;
        
        public bool IsOpen { get; private set; }

        public Platform ParentPlatform { get; private set; }

        public void AttachToPlatform(Platform parentPlatform)
        {
            ParentPlatform = parentPlatform;
            transform.parent = ParentPlatform.transform;
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