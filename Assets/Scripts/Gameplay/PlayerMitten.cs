using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HexGame.Gameplay
{
    public class PlayerMitten : MonoBehaviour, IResetState
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [Header("Launch")]
        [SerializeField] private float _launchSpeed = 0.5f;
        [SerializeField] private AnimationCurve _launchSpeedCurve = AnimationCurve.Linear(1, 1, 2, 2);
        [Header("Pull")]
        [SerializeField] private float _pullSpeed = 0.5f;
        [SerializeField] private AnimationCurve _pullSpeedCurve = AnimationCurve.Linear(1, 1, 2, 2);
        
        public event Action Reset;

        public bool IsPreviewMode { get; private set; }
        public bool IsLaunching { get; private set; }
        public bool IsPulling { get; private set; }
        public Button TargetButton { get; private set; }

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

        public void TogglePreviewMode(bool value)
        {
            IsPreviewMode = value;
            if (value)
            {
                var color = _spriteRenderer.color;
                color.a = 0.6f;
                _spriteRenderer.color = color;
            }
            else
            {
                var color = _spriteRenderer.color;
                color.a = 1;
                _spriteRenderer.color = color;
            }
        }

        public async UniTask LaunchToTarget(Button targetButton, CancellationToken cancellationToken)
        {
            IsLaunching = true;
            
            TargetButton = targetButton;

            UpdatePositionAndRotateToPlayer(_player.transform.position);
            var targetPosition = targetButton.transform.position;
            
            var progress = 0f;
            var maxDistance = GetSqrDistance(transform.position, targetPosition);
            var currentDistance = maxDistance;

            while (currentDistance > 0.001f)
            {
                var maxDistanceDelta = _launchSpeed * Time.deltaTime * _launchSpeedCurve.Evaluate(progress);
                UpdatePositionAndRotateToPlayer(Vector3.MoveTowards(
                    transform.position, targetPosition, maxDistanceDelta));

                currentDistance = GetSqrDistance(transform.position, targetPosition);
                progress = 1 - currentDistance / maxDistance;

                var isCancelled = await UniTask.DelayFrame(1, cancellationToken: cancellationToken)
                    .SuppressCancellationThrow();
                if (isCancelled)
                    break;
            }

            if (!cancellationToken.IsCancellationRequested)
            {
                TargetButton.Press();
            }
            
            IsLaunching = false;
        }

        public async UniTask PullToPlayer(CancellationToken cancellationToken)
        {
            IsPulling = true;
            
            TargetButton.Release();
            TargetButton = null;

            var progress = 0f;
            var maxDistance = GetSqrDistanceToPlayer();
            var currentDistance = maxDistance;

            while (currentDistance > 0.001f)
            {
                var maxDistanceDelta = _pullSpeed * Time.deltaTime * _pullSpeedCurve.Evaluate(progress);
                UpdatePositionAndRotateToPlayer(Vector3.MoveTowards(
                    transform.position, _player.transform.position, maxDistanceDelta));

                currentDistance = GetSqrDistanceToPlayer();
                progress = 1 - currentDistance / maxDistance;

                var isCancelled = await UniTask.DelayFrame(1, cancellationToken: cancellationToken)
                    .SuppressCancellationThrow();
                if (isCancelled)
                    break;
            }
            
            IsPulling = false;
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
            TargetButton = null;
            Hide();
            PullToPlayerImmediate();
            Reset?.Invoke();
        }
        
        private void RotateToPlayer()
        {
            var newMittenRotation = 
                Quaternion.FromToRotation(Vector3.up, transform.position - _player.transform.position);
            transform.rotation = newMittenRotation;
        }
        
        private float GetSqrDistance(Vector3 a, Vector3 b)
        {
            return (a - b).sqrMagnitude;
        }

        private float GetSqrDistanceToPlayer()
        {
            return (transform.position - _player.transform.position).sqrMagnitude;
        }
    }
}