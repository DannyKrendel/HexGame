using DG.Tweening;
using UnityEngine;

namespace HexGame.UI.Animation
{
    public class CellHighlightAnimation : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private Ease _ease = Ease.InOutSine;

        private Tween _tween;
        
        private void Awake()
        {
            _tween = _spriteRenderer
                .DOFade(0, _duration)
                .SetEase(_ease)
                .SetLoops(-1, LoopType.Yoyo)
                .Pause();
        }

        private void OnEnable()
        {
            _tween.Restart();
        }

        private void OnDisable()
        {
            _tween.Pause();
        }
    }
}