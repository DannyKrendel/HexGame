using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexGame.Gameplay
{
    [ExecuteAlways, RequireComponent(typeof(Grid))]
    public class HexGrid : MonoBehaviour
    {
        [SerializeField, HideInInspector] private HexCell[] _cells;
        [SerializeField, HideInInspector] private Grid _grid;
        [SerializeField, HideInInspector] private Bounds _bounds;

        public HexCell[] Cells => _cells;
        public Bounds Bounds => _bounds;

        private Dictionary<HexCoordinates, HexCell> _cellDictionary;

        private void Awake()
        {
            _grid = GetComponent<Grid>();
            _cellDictionary = _cells.ToDictionary(c => c.Coordinates);
        }

        private void Update()
        {
            #if UNITY_EDITOR
            if (!Application.IsPlaying(gameObject))
            {
                UpdateCells();
                CalculateBounds();
            }
            #endif
        }

        public bool TryGetWorldPosition(HexCoordinates coordinates, out Vector3 position)
        {
            position = Vector3.zero;
            
            var foundCell = _cells.FirstOrDefault(x => x.Coordinates == coordinates);
            if (foundCell == null) return false;
            
            position = foundCell.transform.position;
            return true;
        }

        public bool TryGetCell(Vector3 worldPosition, out HexCell cell)
        {
            var cellPos = _grid.WorldToCell(worldPosition);
            var cellCoords = HexCoordinates.FromOffsetCoordinates(cellPos.x, cellPos.y);
            return _cellDictionary.TryGetValue(cellCoords, out cell);;
        }

        public bool TryGetCell(HexCoordinates coordinates, out HexCell cell)
        {
            return _cellDictionary.TryGetValue(coordinates, out cell);
        }

        public List<HexCell> GetNeighbors(HexCoordinates coordinates, int range = 1)
        {
            var neighbors = new List<HexCell>();

            foreach (var cell in _cells)
            {
                if (cell.Coordinates == coordinates) continue;
                
                var diff = cell.Coordinates - coordinates;
                var distance = Mathf.Max(Mathf.Abs(diff.X), Mathf.Abs(diff.Y), Mathf.Abs(diff.Z));
                if (distance <= range) neighbors.Add(cell);
            }

            return neighbors;
        }

        #if UNITY_EDITOR
        private void UpdateCells()
        {
            _cells = GetComponentsInChildren<HexCell>();
        }

        private void CalculateBounds()
        {
            if (_cells == null || _cells.Length == 0) return;
            
            var cellSize = _grid.cellSize;
            _bounds = new Bounds();

            foreach (var cell in _cells)
                _bounds.Encapsulate(new Bounds(cell.transform.position, cellSize));
        }
        #endif
    }
}