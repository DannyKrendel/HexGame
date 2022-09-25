using System;
using DG.Tweening;
using HexGame.Gameplay;
using UnityEngine;

namespace HexGame.UI.Animation
{
    public class CellHighlightAnimation : MonoBehaviour
    {
        [SerializeField] private HexCell _cell;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private Ease _ease = Ease.InOutSine;

        private Tween _tween;
        
        private void Awake()
        {
            _tween = _spriteRenderer
                .DOFade(_spriteRenderer.color.a, _duration)
                .From(0)
                .SetEase(_ease)
                .SetLoops(-1, LoopType.Yoyo)
                .Pause();
        }

        private void OnEnable()
        {
            _cell.Highlighted += Play;
            _tween.Rewind();
        }

        private void OnDisable()
        {
            _cell.Highlighted -= Play;
        }

        private void Play()
        {
            _tween.Restart();
        }
    }
}