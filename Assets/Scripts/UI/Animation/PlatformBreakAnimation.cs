using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using HexGame.Gameplay;
using UnityEngine;

namespace HexGame.UI.Animation
{
    public class PlatformBreakAnimation : MonoBehaviour
    {
        [SerializeField] private Platform _platform;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private Ease _ease = Ease.InOutSine;
        
        private Tween _tween;
        
        private void Awake()
        {
            _tween = _platform.transform
                .DOScale(0, _duration)
                .SetEase(_ease)
                .Pause();
        }

        private void OnEnable()
        {
            _platform.Breaking += Play;
            _tween.Rewind();
        }

        private void OnDisable()
        {
            _platform.Breaking -= Play;
        }

        private async UniTask Play(CancellationToken cancellationToken = default)
        {
            _tween.Restart();
            await _tween.AwaitForComplete(TweenCancelBehaviour.Complete, cancellationToken);
        }
    }
}