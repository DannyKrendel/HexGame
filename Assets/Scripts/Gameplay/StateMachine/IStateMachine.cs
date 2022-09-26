using System;

namespace HexGame.Gameplay.StateMachine
{
    public interface IStateMachine
    {
        event Action StateChanged;
        
        void Initialize();
    }
}