using TMPro;
using UnityEngine;

namespace HexGame.UI
{
    public class HexGridView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TMP_Text _hexCellLabelPrefab;
        [SerializeField] private HexGrid _hexGrid;
        [SerializeField] private GameCamera _gameCamera;

        private void Start()
        {
            CreateLabels();
        }

        private void CreateLabels()
        {
            foreach (var cell in _hexGrid.Cells)
            {
                var label = Instantiate(_hexCellLabelPrefab, _canvas.transform);
                label.rectTransform.position = _gameCamera.Camera.WorldToScreenPoint(cell.transform.position);
                label.text = cell.Coordinates.ToStringOnSeparateLines();
            }
        }
    }
}