using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HexGame.Gameplay
{
    public class Platform : HexGridElement, IHighlight, IBreakable
    {
        [SerializeField, Range(0, 10)] private int _startDurability = 1;
        [SerializeField] private GameObject _highlightObject;
        
        public event Action Highlighted;
        public event Action HighlightCleared;
        public event Func<UniTask> Broke;
        public event Action Reset;
        
        public bool IsHighlighted { get; private set; }
        public int Durability { get; private set; }
        
        private void Awake()
        {
            Durability = _startDurability;
        }
        
        public void Highlight()
        {
            if (IsHighlighted) return;
            _highlightObject.SetActive(true);
            IsHighlighted = true;
            Highlighted?.Invoke();
        }
        
        public void ClearHighlight()
        {
            if (!IsHighlighted) return;
            _highlightObject.SetActive(false);
            IsHighlighted = false;
            HighlightCleared?.Invoke();
        }

        public void SubtractDurability(int amount = 1)
        {
            Durability = Mathf.Max(Durability - amount, 0);
            if (Durability == 0)
                Break().Forget();
        }

        private async UniTaskVoid Break()
        {
            ClearHighlight();
            if (Broke != null) 
                await Broke();
            gameObject.SetActive(false);
        }

        public void ResetState()
        {
            ClearHighlight();
            Durability = _startDurability;
            gameObject.SetActive(true);
            Reset?.Invoke();
        }
    }
}