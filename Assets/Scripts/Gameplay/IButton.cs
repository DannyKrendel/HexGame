using System;

namespace HexGame.Gameplay
{
    public interface IButton
    {
        event Action Pressed;
        event Action Released;
        
        bool IsPressed { get; }

        void Press();
        void Release();
    }
}