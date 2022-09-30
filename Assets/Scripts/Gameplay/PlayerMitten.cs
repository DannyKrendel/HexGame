using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HexGame.Gameplay
{
    public class PlayerMitten : MonoBehaviour, IResetState
    {
        [SerializeField] private float _pullSpeed = 0.5f;
        [SerializeField] private AnimationCurve _speedCurve = AnimationCurve.Linear(1, 1, 2, 2);
        
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

        public async UniTask PullToPlayer(CancellationToken cancellationToken = default)
        {
            var linkedTokenSource = 
                CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, this.GetCancellationTokenOnDestroy());
            var progress = 0f;
            var maxDistance = GetSqrDistanceToPlayer();
            var currentDistance = maxDistance;

            while (currentDistance > 0.001f)
            {
                var maxDistanceDelta = _pullSpeed * Time.deltaTime * _speedCurve.Evaluate(progress);
                UpdatePositionAndRotateToPlayer(Vector3.MoveTowards(
                    transform.position, _player.transform.position, maxDistanceDelta));

                currentDistance = GetSqrDistanceToPlayer();
                progress = 1 - currentDistance / maxDistance;

                await UniTask.Yield(linkedTokenSource.Token);
            }
        }

        public void PullToPlayerImmediate()
        {
            transform.position = _player.transform.position;
        }

        public void UpdatePositionAndRotateToPlayer(Vector3 position)
        {
            transform.position = position;
            RotateToPlayer();
        }

        public void ResetState()
        {
            Hide();
            Reset?.Invoke();
        }
        
        private void RotateToPlayer()
        {
            var newMittenRotation = 
                Quaternion.FromToRotation(Vector3.up, transform.position - _player.transform.position);
            transform.rotation = newMittenRotation;
        }

        private float GetSqrDistanceToPlayer()
        {
            return (transform.position - _player.transform.position).sqrMagnitude;
        }
    }
}