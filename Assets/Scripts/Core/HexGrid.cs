using System;
using UnityEngine;

namespace HexGame
{
    public class HexGrid : MonoBehaviour
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private HexCell _cellPrefab;

        public event Action CellsCreated;
        public HexCell[] Cells { get; private set; }

        private void Awake()
        {
            Cells = new HexCell[_width * _height];
            for (int z = 0, i = 0; z < _height; z++)
                for (int x = 0; x < _width; x++)
                    CreateCell(x, z, i++);
            
            CellsCreated?.Invoke();
        }

        private void CreateCell(int x, int z, int i)
        {
            var position = new Vector3((x + z * 0.5f - z / 2) * (HexCell.InnerRadius * 2f), 0, z * HexCell.OuterRadius * 1.5f);

            Cells[i] = Instantiate(_cellPrefab, transform);
            Cells[i].transform.localPosition = position;
            Cells[i].Coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        }
    }
}