using System;
using System.Linq;
using HexGame.Utils;
using UnityEngine;

namespace HexGame
{
    public class HexGrid : MonoBehaviour
    {
        [SerializeField, Range(1, 50)] private int _width = 5;
        [SerializeField, Range(1, 50)] private int _height = 5;
        [SerializeField, Range(0.1f, 10f)] private float _cellOuterRadius = 1;
        [SerializeField, Range(-1, 1)] private float _margin = 0.1f;
        [SerializeField, HideInInspector] private HexCell[] _cells;
        [SerializeField, HideInInspector] private float _cellInnerRadius;

        public HexCell[] Cells { get => _cells; set => _cells = value; }
        public Bounds Bounds { get; private set; }
        public int Width => _width;
        public int Height => _height;
        public float CellOuterRadius => _cellOuterRadius;
        public float CellInnerRadius => _cellInnerRadius;
        public float Margin => _margin;

        public event Action Validate;

        private void Awake()
        {
            if (Application.IsPlaying(gameObject))
            {
                CalculateBounds();
            }
        }
        
        private void OnValidate()
        {
            CalculateInnerRadius();
            Validate?.Invoke();
        }

        private void CalculateInnerRadius()
        {
            _cellInnerRadius = _cellOuterRadius * 0.866025404f;
        }

        private void CalculateBounds()
        {
            var width = _width * (_cellInnerRadius * 2f) + _margin * (_width - 1) + (_height > 1 ? _margin * 0.5f + _cellInnerRadius : 0);
            var height = _height * (_cellOuterRadius * 1.5f) + (_cellOuterRadius * 0.5f) + _margin * Mathf.Cos(30 * Mathf.Deg2Rad) * (_height - 1);
            var center = transform.position;
            center.x += width * 0.5f - _cellInnerRadius;
            center.z += height * 0.5f - _cellOuterRadius;
            var size = new Vector3(width, height);
            Bounds = new Bounds(center, size);
        }
    }
}