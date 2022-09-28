using System;

namespace HexGame.Gameplay
{
    public interface IResetState
    {
        event Action Reset;
        void ResetState();
    }
}