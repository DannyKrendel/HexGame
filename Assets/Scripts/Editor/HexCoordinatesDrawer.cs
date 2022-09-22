using UnityEditor;
using UnityEngine;

namespace HexGame.Editor
{
    [CustomPropertyDrawer(typeof(HexCoordinates))]
    public class HexCoordinatesDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            var xProp = property.FindPropertyRelative("_x");
            var zProp = property.FindPropertyRelative("_z");
            
            var coordinates = new HexCoordinates(xProp.intValue, zProp.intValue);
            
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            position.x += EditorGUIUtility.labelWidth;
            position.width -= EditorGUIUtility.labelWidth;
            var vector3 = EditorGUI.Vector3IntField(position, GUIContent.none, new Vector3Int(coordinates.X, coordinates.Y, coordinates.Z));

            if (EditorGUI.EndChangeCheck())
            {
                xProp.intValue = vector3.x;
                zProp.intValue = vector3.z;
            }
            
            EditorGUI.EndProperty();
        }
    }
}