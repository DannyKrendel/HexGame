using System;
using UnityEditor;
using UnityEngine;

namespace HexGame.Editor
{
    [CustomEditor(typeof(HexGrid))]
    public class HexGridEditor : UnityEditor.Editor
    {
        private HexGrid _hexGrid;

        private void OnEnable()
        {
            _hexGrid = target as HexGrid;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Fill Grid")) _hexGrid.FillGrid();
                if (GUILayout.Button("Clear Grid")) _hexGrid.ClearGrid();
            }
        }
    }
}