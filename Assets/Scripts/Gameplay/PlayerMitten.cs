using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace HexGame.Gameplay
{
    public class PlayerMitten : MonoBehaviour, IResetState
    {
        [SerializeField] private float _pullDuration = 0.5f;
        [SerializeField] private Ease _pullEase = Ease.Linear;
        
        public event Action Reset;

        private Player _player;

        public void SetPlayer(Player player)
        {
            _player = player;
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public async UniTask PullToPlayer(bool immediate = false)
        {
            if (immediate)
            {
                transform.position = _player.transform.position;
                return;
            }

            var timer = 0f;
            var startPosition = transform.position;
            while (timer < _pullDuration)
            {
                transform.position = Vector3.Lerp(startPosition, _player.transform.position, timer / _pullDuration);
                timer += Time.deltaTime;
                await UniTask.DelayFrame(1);
            }
            
            transform.position = _player.transform.position;
            transform.rotation = Quaternion.identity;
        }

        public void UpdatePositionAndRotate(Vector3 position)
        {
            transform.position = position;

            var newMittenRotation =
                Quaternion.FromToRotation(Vector3.up, transform.position - _player.transform.position);
            transform.rotation = newMittenRotation;
        }

        public void ResetState()
        {
            Hide();
            Reset?.Invoke();
        }
    }
}