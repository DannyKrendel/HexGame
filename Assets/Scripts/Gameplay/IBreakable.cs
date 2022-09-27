using System;
using Cysharp.Threading.Tasks;

namespace HexGame.Gameplay
{
    public interface IBreakable
    {
        event Action Broke;
        int Durability { get; }
        void SubtractDurability(int amount);
    }
}