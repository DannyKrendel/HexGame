using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace HexGame.UI.Animation
{
    public class FadeAnimation : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [Header("Fade In")]
        [SerializeField] private float _fadeInDuration = 0.5f;
        [SerializeField] private Ease _fadeInEase = Ease.InSine;
        [Header("Fade Out")]
        [SerializeField] private float _fadeOutDuration = 0.5f;
        [SerializeField] private Ease _fadeOutEase = Ease.OutSine;

        private Tween _fadeInTween;
        private Tween _fadeOutTween;
        
        private void Awake()
        {
            _fadeInTween = _canvasGroup
                .DOFade(1, _fadeInDuration)
                .From(0)
                .SetEase(_fadeInEase)
                .Pause();
            
            _fadeOutTween = _canvasGroup
                .DOFade(0, _fadeOutDuration)
                .From(1)
                .SetEase(_fadeOutEase)
                .Pause();
        }
        
        public async UniTask PlayFadeIn()
        {
            _fadeInTween.Restart();
            await _fadeInTween.AsyncWaitForCompletion();
        }
        
        public async UniTask PlayFadeOut()
        {
            _fadeOutTween.Restart();
            await _fadeOutTween.AsyncWaitForCompletion();
        }
    }
}