using System;
using System.Collections.Generic;
using System.Linq;
using HexGame.Utils;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace HexGame
{
    [ExecuteAlways]
    public class HexGrid : MonoBehaviour
    {
        [SerializeField, Range(1, 50)] private int _width = 5;
        [SerializeField, Range(1, 50)] private int _height = 5;
        [SerializeField, Range(0.1f, 10f)] private float _cellOuterRadius = 1;
        [SerializeField, Range(-1, 1)] private float _margin = 0.1f;
        [SerializeField] private GameObject _cellPrefab;
        [SerializeField, HideInInspector] private HexCell[] _cells;
        [SerializeField, HideInInspector] private float _cellInnerRadius;
        
        #if UNITY_EDITOR
        [SerializeField, HideInInspector] private Vector3 _cellBoundsSize;
        [SerializeField, HideInInspector] private EditorCellData[] _editorCellDataArray;
        #endif

        public HexCell[] Cells => _cells;
        public Bounds Bounds { get; private set; }

        #if UNITY_EDITOR
        private EditorCellData? _selectedCell;
        #endif

        private void Awake()
        {
            if (Application.IsPlaying(gameObject))
            {
                CalculateBounds();
            }
        }

        private void OnEnable()
        {
            #if UNITY_EDITOR
            if (!Application.IsPlaying(gameObject))
            {
                SceneView.duringSceneGui += OnSceneGui;
                EditorApplication.hierarchyChanged += OnHierarchyChanged;
                _cellBoundsSize = _cellPrefab.GetComponent<HexCell>().SpriteRenderer.bounds.size;
            }
            #endif
        }

        private void OnDisable()
        {
            #if UNITY_EDITOR
            if (!Application.IsPlaying(gameObject))
            {
                SceneView.duringSceneGui -= OnSceneGui;
                EditorApplication.hierarchyChanged -= OnHierarchyChanged;
            }
            #endif
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

        #if UNITY_EDITOR
        private void OnValidate()
        {
            CalculateInnerRadius();
            UpdateEditorCells();
            UpdateCells();
            UpdateCellsSpriteSize();
        }
        
        private void OnHierarchyChanged()
        {
            UpdateCells();
        }
        
        private void OnSceneGui(SceneView sceneView)
        {
            if (_cellPrefab == null) return;

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
            foreach (var cell in _cells)
            {
                UpdateCellSpriteSize(cell);
            }
        }

        private void UpdateCellSpriteSize(HexCell cell)
        {
            cell.SetLocalScale((_cellOuterRadius * 2) / _cellBoundsSize.z);
        }

        private void UpdateEditorCells()
        {
            _editorCellDataArray = new EditorCellData[_width * _height];
            for (int z = 0, i = 0; z < _height; z++)
            for (int x = 0; x < _width; x++)
            {
                var position = HexGridUtils.GetCellPosition(x, z, _cellInnerRadius, _cellOuterRadius, _margin);
                var corners = HexGridUtils.GetCellCorners(position, _cellInnerRadius, _cellOuterRadius);
                _editorCellDataArray[i++] = 
                    new EditorCellData { Coordinates = HexCoordinates.FromOffsetCoordinates(x, z), Position = position, Corners = corners };
            }
        }

        private void UpdateCells()
        {
            _cells = GetComponentsInChildren<HexCell>();
            
            foreach (var cell in _cells)
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
            if (_cells.Any(c => c.Coordinates == editorCellData.Coordinates)) return; 
            
            var cell = ((GameObject)PrefabUtility.InstantiatePrefab(_cellPrefab, transform)).GetComponent<HexCell>();
            cell.transform.localPosition = editorCellData.Position;
            cell.Coordinates = editorCellData.Coordinates;
            UpdateCellSpriteSize(cell);
            
            Undo.RegisterCreatedObjectUndo(cell.gameObject, "Create Cell");
        }

        private void DeleteCell(EditorCellData editorCellData)
        {
            var foundCell = _cells.FirstOrDefault(c => c.Coordinates == editorCellData.Coordinates);
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