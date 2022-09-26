using UnityEngine;

namespace HexGame.Gameplay
{
    public class GridService
    {
        private readonly Grid _grid;

        public Vector3 CellSize => _grid.cellSize;
        
        public GridService(Grid grid)
        {
            _grid = grid;
        }
        
        public Vector3 CoordinatesToWorld(HexCoordinates coordinates)
        {
            var offsetCoordinates = coordinates.ToOffsetCoordinates();
            return _grid.CellToWorld(offsetCoordinates);
        }

        public HexCoordinates WorldToCoordinates(Vector3 worldPosition)
        {
            var cellPos = _grid.WorldToCell(worldPosition);
            return HexCoordinates.FromOffsetCoordinates(cellPos.x, cellPos.y);
        }
    }
}