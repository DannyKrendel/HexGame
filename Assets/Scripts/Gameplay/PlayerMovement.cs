using System;
using UnityEngine;
using Zenject;

namespace HexGame.Gameplay
{
    public class PlayerMovement : MonoBehaviour, IHexGridActor
    {
        [SerializeField] private float _moveDuration = .5f;
        
        public HexCoordinates Coordinates { get; private set; }
        public bool IsMoving { get; private set; }
        public event Action Moved;
        
        private HexGrid _hexGrid;
        private Vector3 _startPosition;
        private Vector3? _destinationPosition;
        private HexCoordinates? _destinationCoordinates;
        private float _moveTimer;

        [Inject]
        private void Construct(HexGrid hexGrid)
        {
            _hexGrid = hexGrid;
        }

        private void Update()
        {
            if (!_destinationPosition.HasValue) return;
            
            if (_moveTimer > 0)
            {
                var progress = (_moveDuration - _moveTimer) / _moveDuration;
                transform.position = Vector3.Lerp(_startPosition, _destinationPosition.Value, progress);
                _moveTimer -= Time.deltaTime;
            }
            else
            {
                StopMoving();
            }
        }

        public void Move(HexCoordinates coordinates, bool immediate = false)
        {
            if (_destinationPosition.HasValue || _destinationCoordinates.HasValue || Coordinates == coordinates) 
                return;
            
            if (_hexGrid.TryGetWorldPosition(coordinates, out var destination))
            {
                StartMoving(destination, coordinates);
                if (immediate)
                    StopMoving();
            }
        }

        private void StartMoving(Vector3 destinationPosition, HexCoordinates destinationCoordinates)
        {
            _startPosition = transform.position;
            _destinationPosition = destinationPosition;
            _destinationCoordinates = destinationCoordinates;
            _moveTimer = _moveDuration;
            IsMoving = true;
        }

        private void StopMoving()
        {
            Coordinates = _destinationCoordinates.GetValueOrDefault();
            transform.position = _destinationPosition.GetValueOrDefault();
            _moveTimer = 0;
            _destinationPosition = null;
            _destinationCoordinates = null;
            IsMoving = false;
            Moved?.Invoke();
        }
    }
}