using System;
using Cysharp.Threading.Tasks;

namespace HexGame.Gameplay
{
    public class Fish : HexGridElement, IConsumable
    {
        public event Func<UniTask> Consumed;
        public event Action Reset;
        
        public bool IsConsumed { get; private set; }

        public async UniTask Consume()
        {
            IsConsumed = true;
            if (Consumed != null)
                await Consumed();
            gameObject.SetActive(false);
        }
        
        public void ResetState()
        {
            IsConsumed = false;
            gameObject.SetActive(true);
            Reset?.Invoke();
        }
    }
}