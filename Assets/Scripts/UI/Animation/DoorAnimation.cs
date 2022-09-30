using System;
using DG.Tweening;
using HexGame.Gameplay;
using UnityEngine;

namespace HexGame.UI.Animation
{
    public class DoorAnimation : MonoBehaviour
    {
        [SerializeField] private Door _door;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private Ease _ease = Ease.InOutSine;

        private float _openAlpha = 0.5f;
        private Tween _openTween;
        private Tween _closeTween;
        
        private void Awake()
        {
            _openTween = _spriteRenderer
                .DOFade(_openAlpha, _duration)
                .SetEase(_ease)
                .Pause();
            _closeTween = _spriteRenderer
                .DOFade(1, _duration)
                .SetEase(_ease)
                .Pause();
        }

        private void OnEnable()
        {
            _door.Opened += PlayOpen;
            _door.Closed += PlayClose;
        }
        
        private void OnDisable()
        {
            _door.Opened -= PlayOpen;
            _door.Closed -= PlayClose;
        }

        private void PlayOpen()
        {
            _openTween.Restart();
        }

        private void PlayClose()
        {
            _closeTween.Restart();
        }
    }
}