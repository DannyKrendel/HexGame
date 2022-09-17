using UnityEditor;
using UnityEngine;

namespace HexGame.Editor
{
    [CustomPropertyDrawer(typeof(HexCoordinates))]
    public class HexCoordinatesDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var coordinates = new HexCoordinates(
                property.FindPropertyRelative("_x").intValue,
                property.FindPropertyRelative("_z").intValue
            );

            EditorGUI.LabelField(position, label.text, coordinates.ToString());
        }
    }
}