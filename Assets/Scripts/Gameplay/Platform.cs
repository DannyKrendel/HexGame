using System;
using UnityEngine;

namespace HexGame.Gameplay
{
    public class Platform : HexGridElement, IHighlight
    {
        [SerializeField, Range(0, 10)] private int _startDurability = 1;
        [SerializeField] private GameObject _highlightObject;
        
        public event Action Highlighted;
        public event Action HighlightCleared;
        public event Action Broke;
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
            {
                ClearHighlight();
                Broke?.Invoke();
            }
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