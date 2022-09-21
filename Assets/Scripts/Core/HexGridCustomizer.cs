using System;
using System.Linq;
using HexGame.Utils;
using UnityEditor.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace HexGame.Core
{
    [ExecuteAlways]
    [RequireComponent(typeof(HexGrid))]
    public class HexGridCustomizer : MonoBehaviour
    {
        #if UNITY_EDITOR
        [SerializeField] private GameObject _cellPrefab;
        [SerializeField, HideInInspector] private HexGrid _hexGrid;
        [SerializeField, HideInInspector] private EditorCellData[] _editorCellDataArray;
        [SerializeField, HideInInspector] private bool _allowEditing;

        private EditorCellData? _selectedCell;
        
        public bool AllowEditing
        {
            get => _allowEditing;
            set
            {
                _allowEditing = value;
                SceneView.RepaintAll();
            }
        }

        private void Awake()
        {
            if (!Application.IsPlaying(gameObject))
            {
                _hexGrid = GetComponent<HexGrid>();
            }
        }

        private void OnEnable()
        {
            if (!Application.IsPlaying(gameObject))
            {
                SceneView.duringSceneGui += OnSceneGui;
                EditorApplication.hierarchyChanged += OnHierarchyChanged;
                _hexGrid.Validate += OnValidate;
            }
        }

        private void OnDisable()
        {
            if (!Application.IsPlaying(gameObject))
            {
                SceneView.duringSceneGui -= OnSceneGui;
                EditorApplication.hierarchyChanged -= OnHierarchyChanged;
                _hexGrid.Validate -= OnValidate;
            }
        }

        private void OnValidate()
        {
            if (!AllowEditing || !_cellPrefab) return; 
            
            UpdateEditorCells();
            UpdateCells();
            UpdateCellsSpriteSize();
        }
        
        private void OnHierarchyChanged()
        {
            if (!AllowEditing || !_cellPrefab) return;
            
            UpdateCells();
        }
        
        private void OnSceneGui(SceneView sceneView)
        {
            if (!AllowEditing || !_cellPrefab || PrefabStageUtility.GetCurrentPrefabStage() != null) return;

            var mousePos = Event.current.mousePosition;
            var ray = HandleUtility.GUIPointToWorldRay(mousePos);
            var plane = new Plane(Vector3.up, Vector3.zero);
            var hitPoint = Vector3.zero;
            if (plane.Raycast(ray, out var distance))
                hitPoint = ray.GetPoint(distance);

            _selectedCell = null;

            for (int i = 0; i < _editorCellDataArray.Length; i++)
            {
                if (!_selectedCell.HasValue && GeometryUtils.IsPointInsidePolygon(hitPoint, _editorCellDataArray[i].Corners))
                {
                    Handles.color = new Color(1, 1, 1, 0.4f);
                    _selectedCell = _editorCellDataArray[i];
                }
                else
                {
                    Handles.color = new Color(1, 1, 1, 0.2f);
                }
                
                Handles.DrawAAConvexPolygon(_editorCellDataArray[i].Corners);
            }

            if (_selectedCell.HasValue && Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                if (Event.current.control)
                    DeleteCell(_selectedCell.Value);
                else
                    CreateCell(_selectedCell.Value);

                Event.current.Use();

                Selection.activeGameObject = null;
            }
            
            SceneView.RepaintAll();
        }

        private void UpdateCellsSpriteSize()
        {
            foreach (var cell in _hexGrid.Cells)
                UpdateCellSpriteSize(cell);
        }

        private void UpdateCellSpriteSize(HexCell cell)
        {
            cell.SetLocalScale((_hexGrid.CellOuterRadius * 2) / cell.BoundsSize.y);
        }

        private void UpdateEditorCells()
        {
            _editorCellDataArray = new EditorCellData[_hexGrid.Width * _hexGrid.Height];
            for (int z = 0, i = 0; z < _hexGrid.Height; z++)
            for (int x = 0; x < _hexGrid.Width; x++)
            {
                var position = HexGridUtils.GetCellPosition(x, z, _hexGrid.CellInnerRadius, _hexGrid.CellOuterRadius, _hexGrid.Margin);
                var corners = HexGridUtils.GetCellCorners(position, _hexGrid.CellInnerRadius, _hexGrid.CellOuterRadius);
                _editorCellDataArray[i++] = 
                    new EditorCellData { Coordinates = HexCoordinates.FromOffsetCoordinates(x, z), Position = position, Corners = corners };
            }
        }

        private void UpdateCells()
        {
            _hexGrid.Cells = GetComponentsInChildren<HexCell>();
            
            foreach (var cell in _hexGrid.Cells)
            {
                foreach (var editorCell in _editorCellDataArray)
                {
                    if (editorCell.Coordinates == cell.Coordinates)
                        cell.transform.position = editorCell.Position;
                }
            }
        }

        private void CreateCell(EditorCellData editorCellData)
        {
            if (_hexGrid.Cells.Any(c => c.Coordinates == editorCellData.Coordinates)) return; 
            
            var cell = ((GameObject)PrefabUtility.InstantiatePrefab(_cellPrefab, transform)).GetComponent<HexCell>();
            cell.transform.localPosition = editorCellData.Position;
            cell.Coordinates = editorCellData.Coordinates;
            UpdateCellSpriteSize(cell);
            
            Undo.RegisterCreatedObjectUndo(cell.gameObject, "Create Cell");
        }

        private void DeleteCell(EditorCellData editorCellData)
        {
            var foundCell = _hexGrid.Cells.FirstOrDefault(c => c.Coordinates == editorCellData.Coordinates);
            if (foundCell == null) return;
            
            Undo.DestroyObjectImmediate(foundCell.gameObject);
        }

        public void FillGrid()
        {
            foreach (var editorCell in _editorCellDataArray)
                CreateCell(editorCell);
        }

        public void ClearGrid()
        {
            foreach (var editorCell in _editorCellDataArray)
                DeleteCell(editorCell);
        }

        [Serializable]
        private struct EditorCellData
        {
            public HexCoordinates Coordinates;
            public Vector3 Position;
            public Vector3[] Corners;
        }
        #endif
    }
}