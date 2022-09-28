using System;
using Cysharp.Threading.Tasks;
using HexGame.Utils;

namespace HexGame.Gameplay
{
    public class Fish : HexGridElement, IConsumable
    {
        public event ActionAsync Consuming;
        public event Action Consumed;
        public event Action Reset;
        
        public bool IsConsumed { get; private set; }

        public async UniTask Consume()
        {
            IsConsumed = true;
            await Consuming.InvokeAsync();
            gameObject.SetActive(false);
            Consumed?.Invoke();
        }
        
        public void ResetState()
        {
            IsConsumed = false;
            gameObject.SetActive(true);
            Reset?.Invoke();
        }
    }
}