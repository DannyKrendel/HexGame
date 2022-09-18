using System;
using HexGame.Utils;
using UnityEngine;

namespace HexGame
{
    public class HexGrid : MonoBehaviour
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private float _margin;
        [SerializeField] private HexCell _cellPrefab;

        public event Action CellsCreated;
        public HexCell[] Cells { get; private set; }
        public Bounds Bounds { get; private set; }

        private float _cellOuterRadius;
        private float _cellInnerRadius;

        private void Awake()
        {
            _cellOuterRadius = _cellPrefab.SpriteRenderer.bounds.extents.z;
            _cellInnerRadius = _cellOuterRadius * 0.866025404f;
            CalculateBounds();
            CreateCells();
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

        private void CreateCells()
        {
            Cells = new HexCell[_width * _height];
            for (int z = 0, i = 0; z < _height; z++)
            for (int x = 0; x < _width; x++)
                CreateCell(x, z, i++);

            CellsCreated?.Invoke();
        }

        private void CreateCell(int x, int z, int i)
        {
            var position = HexGridUtils.GetCellPosition(x, z, _cellInnerRadius, _cellOuterRadius, _margin);

            Cells[i] = Instantiate(_cellPrefab, transform);
            Cells[i].transform.localPosition = position;
            Cells[i].Coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        }
    }
}