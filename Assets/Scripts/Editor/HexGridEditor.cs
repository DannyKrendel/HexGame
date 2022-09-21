using HexGame.Core;
using UnityEditor;
using UnityEngine;

namespace HexGame.Editor
{
    [CustomEditor(typeof(HexGrid))]
    public class HexGridEditor : UnityEditor.Editor
    {
        private HexGrid _hexGrid;
        private HexGridCustomizer _hexGridCustomizer;

        private void OnEnable()
        {
            _hexGrid = target as HexGrid;
            _hexGrid.TryGetComponent(out _hexGridCustomizer);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (_hexGridCustomizer == null) return;
            
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Fill Grid")) _hexGridCustomizer.FillGrid();
                if (GUILayout.Button("Clear Grid")) _hexGridCustomizer.ClearGrid();
            }
        }
    }
}