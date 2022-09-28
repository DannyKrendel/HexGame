using System;

namespace HexGame.Gameplay
{
    public interface IBreakable
    {
        event Action DurabilityChanged;
        event Action Broke;
        int Durability { get; }
        void SubtractDurability(int amount);
    }
}