using UnityEngine;
using Zenject;

namespace HexGame.Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        private HexGrid _hexGrid;
        
        public HexCoordinates Coordinates { get; private set; }

        [Inject]
        private void Construct(HexGrid hexGrid)
        {
            _hexGrid = hexGrid;
        }

        public void MoveTo(HexCoordinates coordinates)
        {
            Coordinates = coordinates;
            if (_hexGrid.TryGetWorldPosition(Coordinates, out var position))
                transform.position = position;
        }
    }
}