using DG.Tweening;
using HexGame.Gameplay;
using UnityEngine;

namespace HexGame.UI.Animation
{
    public class PlatformHighlightAnimation : MonoBehaviour
    {
        [SerializeField] private Platform _platform;
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
            _platform.Highlighted += Play;
            _tween.Rewind();
        }

        private void OnDisable()
        {
            _platform.Highlighted -= Play;
        }

        private void Play()
        {
            _tween.Restart();
        }
    }
}