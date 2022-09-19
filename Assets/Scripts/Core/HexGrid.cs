using System;
using System.Collections.Generic;
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
        [SerializeField, Range(-1, 1)] private float _margin = 0.1f;
        [SerializeField] private HexCell _cellPrefab;
        
        #if UNITY_EDITOR
        [SerializeField, HideInInspector] private EditorCellData[] _editorCellDataArray;
        #endif
        
        public List<HexCell> Cells { get; } = new();
        public Bounds Bounds { get; private set; }

        private float _cellOuterRadius;
        private float _cellInnerRadius;

        private void Awake()
        {
            if (!Application.IsPlaying(gameObject)) return;
            
            _cellOuterRadius = _cellPrefab.SpriteRenderer.bounds.extents.z;
            _cellInnerRadius = _cellOuterRadius * 0.866025404f;
            CalculateBounds();
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            CreateEditorCells();
        }
        #endif

        private void OnEnable()
        {
            #if UNITY_EDITOR
            if (!Application.IsPlaying(gameObject))
            {
                SceneView.duringSceneGui += OnSceneGui;
            }
            #endif
        }
        
        private void OnDisable()
        {
            #if UNITY_EDITOR
            if (!Application.IsPlaying(gameObject))
            {
                SceneView.duringSceneGui -= OnSceneGui;
            }
            #endif
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
        private void OnSceneGui(SceneView sceneView)
        {
            if (_cellPrefab == null) return;
            
            for (int i = 0; i < _editorCellDataArray.Length; i++)
            {
                Handles.color = new Color(1, 1, 1, 0.2f);
                Handles.DrawAAConvexPolygon(_editorCellDataArray[i].Corners);
            }
        }
        
        private void CreateEditorCells()
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
        
        private void CreateCell(int x, int z, int i)
        {
            var position = HexGridUtils.GetCellPosition(x, z, _cellInnerRadius, _cellOuterRadius, _margin);
            
            var cell = Instantiate(_cellPrefab, transform);
            cell.transform.localPosition = position;
            cell.Coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
            Cells.Add(cell);
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