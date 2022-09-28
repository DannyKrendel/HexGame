using System;

namespace HexGame.Gameplay
{
    public interface IDoor
    {
        event Action Opened;
        event Action Closed;
        
        bool IsOpen { get; }

        void Open();
        void Close();
    }
}