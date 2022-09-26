using System;
using Cysharp.Threading.Tasks;

namespace HexGame.Gameplay
{
    public interface IBreakable
    {
        event Func<UniTask> Broke;
        int Durability { get; }
        void SubtractDurability(int amount);
    }
}