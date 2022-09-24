﻿using System;
using DG.Tweening;
using HexGame.Gameplay;
using UnityEngine;

namespace HexGame.UI.Animation
{
    public class CellBreakAnimation : MonoBehaviour
    {
        [SerializeField] private HexCell _cell;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private Ease _ease = Ease.InOutSine;
        
        private Tween _tween;
        
        private void Awake()
        {
            _tween = _spriteRenderer.transform
                .DOScale(0, _duration)
                .SetEase(_ease)
                .OnComplete(() => _cell.gameObject.SetActive(false))
                .Pause()
                .SetAutoKill(false);
        }

        private void OnEnable()
        {
            _cell.Broke += Play;
            _cell.Reset += ResetState;
        }

        private void OnDisable()
        {
            _cell.Broke -= Play;
            _cell.Reset -= ResetState;
        }

        private void Play()
        {
            _tween.Restart();
        }
        
        private void ResetState()
        {
            _tween.Rewind();
        }
    }
}