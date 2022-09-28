using System;
using Cysharp.Threading.Tasks;

namespace HexGame.Gameplay
{
    public interface IConsumable
    {
        event Action Consumed;
        bool IsConsumed { get; }
        
        UniTask Consume();
    }
}