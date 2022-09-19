using TMPro;
using UnityEngine;

namespace HexGame.UI
{
    public class HexGridView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TMP_Text _hexCellLabelPrefab;
        [SerializeField] private HexGrid _hexGrid;
        [SerializeField] private CameraController _cameraController;

        private void Start()
        {
            _cameraController.FitCameraToBounds(_hexGrid.Bounds);
            CreateLabels();
        }

        private void CreateLabels()
        {
            foreach (var cell in _hexGrid.Cells)
            {
                var label = Instantiate(_hexCellLabelPrefab, _canvas.transform);
                label.rectTransform.position = _cameraController.Camera.WorldToScreenPoint(cell.transform.position);
                label.text = cell.Coordinates.ToStringOnSeparateLines();
            }
        }
    }
}