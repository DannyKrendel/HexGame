using System;

namespace HexGame.Gameplay
{
    public interface IBreakable
    {
        event Action DurabilityChanged;
        event Action Broke;
        bool IsBroken { get; }
        int Durability { get; }
        void SubtractDurability(int amount);
    }
}