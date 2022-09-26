using System;
using DG.Tweening;
using HexGame.Gameplay;
using UnityEngine;

namespace HexGame.UI.Animation
{
    public class PlatformBreakAnimation : MonoBehaviour
    {
        [SerializeField] private Platform _platform;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private Ease _ease = Ease.InOutSine;
        
        private Tween _tween;
        
        private void Awake()
        {
            _tween = _spriteRenderer.transform
                .DOScale(0, _duration)
                .SetEase(_ease)
                .OnComplete(() => _platform.gameObject.SetActive(false))
                .Pause();
        }

        private void OnEnable()
        {
            _platform.Broke += Play;
            _tween.Rewind();
        }

        private void OnDisable()
        {
            _platform.Broke -= Play;
        }

        private void Play()
        {
            _tween.Restart();
        }
    }
}