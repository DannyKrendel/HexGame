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
                var bgColor = GUI.backgroundColor;
                
                if (_hexGridCustomizer.AllowEditing)
                    GUI.backgroundColor = new Color(0.6f, 0.6f, 0.6f);
                if (GUILayout.Button("Editing: " + (_hexGridCustomizer.AllowEditing ? "On" : "Off")))
                    _hexGridCustomizer.AllowEditing = !_hexGridCustomizer.AllowEditing;
                
                GUI.backgroundColor = bgColor;
                
                if (GUILayout.Button("Fill Grid")) _hexGridCustomizer.FillGrid();
                if (GUILayout.Button("Clear Grid")) _hexGridCustomizer.ClearGrid();
            }
        }
    }
}