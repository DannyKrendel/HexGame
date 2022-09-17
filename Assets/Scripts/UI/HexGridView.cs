using TMPro;
using UnityEngine;

namespace HexGame.UI
{
    public class HexGridView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TMP_Text _hexCellLabelPrefab;
        [SerializeField] private HexGrid _hexGrid;

        private void OnEnable()
        {
            _hexGrid.CellsCreated += OnCellsCreated;
        }
        
        private void OnDisable()
        {
            _hexGrid.CellsCreated -= OnCellsCreated;
        }

        private void OnCellsCreated()
        {
            foreach (var cell in _hexGrid.Cells)
            {
                var label = Instantiate(_hexCellLabelPrefab, _canvas.transform);
                label.rectTransform.position = Camera.main.WorldToScreenPoint(cell.transform.position);
                label.text = cell.Coordinates.ToStringOnSeparateLines();
            }
        }
    }
}