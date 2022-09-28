using System;

namespace HexGame.Gameplay
{
    public interface IConsumable
    {
        event Action Consumed;
        bool IsConsumed { get; }
        
        void Consume();
    }
}