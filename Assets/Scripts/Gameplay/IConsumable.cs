using System;
using Cysharp.Threading.Tasks;

namespace HexGame.Gameplay
{
    public interface IConsumable
    {
        event Func<UniTask> Consumed;
        bool IsConsumed { get; }
        
        UniTask Consume();
    }
}