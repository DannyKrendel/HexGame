using UnityEditor;
using UnityEngine;

namespace HexGame.Editor
{
    [CustomPropertyDrawer(typeof(HexCoordinates))]
    public class HexCoordinatesDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                var xProp = property.FindPropertyRelative("_x");
                var zProp = property.FindPropertyRelative("_z");

                EditorGUI.LabelField(position, label);

                position.x += EditorGUIUtility.labelWidth;
                position.width -= EditorGUIUtility.labelWidth;

                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    var coordinates = new HexCoordinates(xProp.intValue, zProp.intValue);
                    var vector3 = EditorGUI.Vector3IntField(position, GUIContent.none,
                        new Vector3Int(coordinates.X, coordinates.Y, coordinates.Z));

                    if (check.changed)
                    {
                        xProp.intValue = vector3.x;
                        zProp.intValue = vector3.z;
                    }
                }
            }
            EditorGUI.showMixedValue = false;
        }
    }
}