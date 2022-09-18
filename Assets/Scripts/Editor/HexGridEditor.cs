using System;
using HexGame.Utils;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HexGame.Editor
{
    public class HexGridEditor : EditorWindow
    {
        private const string EditorPrefs_HexGridWidth = "HexGridWidth";
        private const string EditorPrefs_HexGridHeight = "HexGridHeight";
        private const string EditorPrefs_CellOuterRadius = "CellOuterRadius";
        private const string EditorPrefs_CellPrefabGUID = "CellPrefabId";
        
        private int _width;
        private int _height;
        private float _cellOuterRadius;
        private float _cellInnerRadius;
        private GameObject _cellPrefab;

        [MenuItem("Window/Hex Grid Editor")]
        private static void Init() => GetWindow<HexGridEditor>("Hex Grid Editor");

        private void OnEnable()
        {
            LoadSettings();
            SceneView.duringSceneGui += OnSceneGui;
        }
        
        private void OnDisable()
        {
            SaveSettings();
            SceneView.duringSceneGui -= OnSceneGui;
            SceneView.RepaintAll();
        }

        private void OnGUI()
        {
            NumberField("Grid Width", ref _width, 0, 50);
            NumberField("Grid Height", ref _height, 0, 50);
            if (NumberField("Cell Outer Radius", ref _cellOuterRadius))
                _cellInnerRadius = _cellOuterRadius * 0.866025404f;

            _cellPrefab = (GameObject)EditorGUILayout.ObjectField("Cell Prefab", _cellPrefab, typeof(GameObject), false);

            if (GUI.changed) SceneView.RepaintAll();
        }

        private void OnSceneGui(SceneView sceneView)
        {
            for (int z = 0, i = 0; z < _height; z++)
            for (int x = 0; x < _width; x++)
            {
                var position = HexGridUtils.GetCellPosition(x, z, _cellInnerRadius, _cellOuterRadius, 0);
                Handles.Disc(Quaternion.identity, position, Vector3.up, _cellOuterRadius, false, 0);
            }
        }

        private void LoadSettings()
        {
            _width = EditorPrefs.GetInt(EditorPrefs_HexGridWidth, 5);
            _height = EditorPrefs.GetInt(EditorPrefs_HexGridHeight, 5);
            _cellOuterRadius = EditorPrefs.GetFloat(EditorPrefs_CellOuterRadius, 1);
            var cellPrefabPath = AssetDatabase.GUIDToAssetPath(EditorPrefs.GetString(EditorPrefs_CellPrefabGUID));
            _cellPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(cellPrefabPath);
        }

        private void SaveSettings()
        {
            EditorPrefs.SetInt(EditorPrefs_HexGridWidth, _width);
            EditorPrefs.SetInt(EditorPrefs_HexGridHeight, _height);
            EditorPrefs.SetFloat(EditorPrefs_CellOuterRadius, _cellOuterRadius);
            if (_cellPrefab == null)
                EditorPrefs.SetString(EditorPrefs_CellPrefabGUID, "");
            else if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(_cellPrefab, out var guid, out long _))
                EditorPrefs.SetString(EditorPrefs_CellPrefabGUID, guid);
        }

        private static bool NumberField<T>(string labelName, ref T value, T? min = null, T? max = null) where T : struct
        {
            T newValue;
            using (new EditorGUILayout.HorizontalScope())
            {
                switch (value)
                {
                    case int v:
                        if (min == null || max == null)
                            newValue = (T)(object)EditorGUILayout.IntField(labelName, v);
                        else
                            newValue = (T)(object)EditorGUILayout.IntSlider(labelName, v, (int)(object)min, (int)(object)max);
                        break;
                    case float v:
                        if (min == null || max == null)
                            newValue = (T)(object)EditorGUILayout.FloatField(labelName, v);
                        else
                            newValue = (T)(object)EditorGUILayout.Slider(labelName, v, (float)(object)min, (float)(object)max);
                        break;
                    default:
                        newValue = default;
                        break;
                }
            }

            var result = !value.Equals(newValue);

            value = newValue;

            return result;
        }
    }
}