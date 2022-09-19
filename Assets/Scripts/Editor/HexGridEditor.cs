using System;
using System.Linq;
using HexGame.Utils;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HexGame.Editor
{
    public class HexGridEditor : EditorWindow
    {
        private EditorOptionInt _width;
        private EditorOptionInt _height;
        private EditorOptionFloat _cellOuterRadius;
        private EditorOptionFloat _cellMargin;
        private EditorOptionGameObject _cellPrefab;
        private EditorOptionColor _cellColorNormal;
        private EditorOptionColor _cellColorSelected;
        
        private float _cellInnerRadius;

        private CellData[] _cellDataArray;

        [MenuItem("Window/Hex Grid Editor")]
        private static void Init() => GetWindow<HexGridEditor>("Hex Grid Editor");

        private void OnEnable()
        {
            InitializeSettings();
            LoadSettings();
            _cellInnerRadius = _cellOuterRadius.Value * 0.866025404f;
            RefreshCellDataArray();
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
            _width.Value = NumberField("Grid Width", _width.Value, 0, 50);
            _height.Value = NumberField("Grid Height", _height.Value, 0, 50);
            
            var lastCellOuterRadius = _cellOuterRadius.Value;
            _cellOuterRadius.Value = NumberField("Cell Outer Radius", lastCellOuterRadius);
            if (_cellOuterRadius.Value != lastCellOuterRadius)
                _cellInnerRadius = _cellOuterRadius.Value * 0.866025404f;

            _cellMargin.Value = NumberField("Cell Margin", _cellMargin.Value, -1, 1);
            _cellPrefab.Value = (GameObject)EditorGUILayout.ObjectField("Cell Prefab", _cellPrefab.Value, typeof(GameObject), false);
            _cellColorNormal.Value = EditorGUILayout.ColorField("Cell Normal Color", _cellColorNormal.Value);
            _cellColorSelected.Value = EditorGUILayout.ColorField("Cell Selected Color", _cellColorSelected.Value);

            if (GUI.changed)
            {
                RefreshCellDataArray();
                SceneView.RepaintAll();
            }
        }

        private void OnSceneGui(SceneView sceneView)
        {
            if (_cellPrefab.Value == null) return;
            
            for (int i = 0; i < _cellDataArray.Length; i++)
            {
                Handles.color = _cellColorNormal.Value;
                Handles.DrawAAConvexPolygon(_cellDataArray[i].Corners);
            }
        }

        private void RefreshCellDataArray()
        {
            _cellDataArray = new CellData[_width.Value * _height.Value];
            
            for (int z = 0, i = 0; z < _height.Value; z++)
            for (int x = 0; x < _width.Value; x++)
            {
                var position = HexGridUtils.GetCellPosition(x, z, _cellInnerRadius, _cellOuterRadius.Value, _cellMargin.Value);
                var corners = HexGridUtils.GetCellPoints(position, _cellInnerRadius, _cellOuterRadius.Value);
                _cellDataArray[i++] = new CellData { Position = position, Corners = corners };
            }
        }

        private void InitializeSettings()
        {
            _width = new EditorOptionInt("HexGridWidth");
            _height = new EditorOptionInt("HexGridHeight");
            _cellOuterRadius = new EditorOptionFloat("CellOuterRadius");
            _cellMargin = new EditorOptionFloat("CellMargin");
            _cellPrefab = new EditorOptionGameObject("CellPrefab");
            _cellColorNormal = new EditorOptionColor("CellColorNormal");
            _cellColorSelected = new EditorOptionColor("CellColorSelected");
        }

        private void LoadSettings()
        {
            _width.Load(5);
            _height.Load(5);
            _cellOuterRadius.Load(1);
            _cellMargin.Load(0.1f);
            _cellPrefab.Load(null);
            _cellColorNormal.Load(new Color(1, 1, 1, 0.2f));
            _cellColorSelected.Load(new Color(1, 1, 1, 0.5f));
        }

        private void SaveSettings()
        {
            _width.Save();
            _height.Save();
            _cellOuterRadius.Save();
            _cellMargin.Save();
            _cellPrefab.Save();
            _cellColorNormal.Save();
            _cellColorSelected.Save();
        }

        private static T NumberField<T>(string labelName, T value, T? min = null, T? max = null) where T : struct
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

            var changed = !value.Equals(newValue);

            value = newValue;

            return value;
        }

        private struct CellData
        {
            public Vector3 Position;
            public Vector3[] Corners;
        }
    }

    public abstract class EditorOption<T>
    {
        protected string EditorPrefsKey;
        
        protected EditorOption(string editorPrefsKey)
        {
            EditorPrefsKey = editorPrefsKey;
        }
        public abstract T Value { get; set; }
        public abstract void Load(T defaultValue);
        public abstract void Save();
    }

    public class EditorOptionInt : EditorOption<int>
    {
        public EditorOptionInt(string editorPrefsKey) : base(editorPrefsKey) { }
        public override int Value { get; set; }
        public override void Load(int defaultValue) => Value = EditorPrefs.GetInt(EditorPrefsKey, defaultValue);
        public override void Save() => EditorPrefs.SetInt(EditorPrefsKey, Value);
    }
    
    public class EditorOptionFloat : EditorOption<float>
    {
        public EditorOptionFloat(string editorPrefsKey) : base(editorPrefsKey) { }
        public override float Value { get; set; }
        public override void Load(float defaultValue) => Value = EditorPrefs.GetFloat(EditorPrefsKey, defaultValue);
        public override void Save() => EditorPrefs.SetFloat(EditorPrefsKey, Value);
    }
    
    public class EditorOptionColor : EditorOption<Color>
    {
        public EditorOptionColor(string editorPrefsKey) : base(editorPrefsKey) { }
        public override Color Value { get; set; }
        public override void Load(Color defaultValue)
        {
            Value = Deserialize(EditorPrefs.GetString(EditorPrefsKey, Serialize(defaultValue)));
        }

        public override void Save()
        {
            EditorPrefs.SetString(EditorPrefsKey, Serialize(Value));
        }

        private string Serialize(Color color)
        {
            return $"{color.r}; {color.g}; {color.b}; {color.a}";
        }

        private Color Deserialize(string str)
        {
            var arr = str.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(float.Parse).ToArray();
            return new Color(arr[0], arr[1], arr[2], arr[3]);
        }
    }
    
    public class EditorOptionGameObject : EditorOption<GameObject>
    {
        public EditorOptionGameObject(string editorPrefsKey) : base(editorPrefsKey) { }
        public override GameObject Value { get; set; }
        public override void Load(GameObject defaultValue)
        {
            Value = Deserialize(EditorPrefs.GetString(EditorPrefsKey, Serialize(defaultValue)));
        }

        public override void Save()
        {
            EditorPrefs.SetString(EditorPrefsKey, Serialize(Value));
        }
        
        private string Serialize(GameObject gameObject)
        {
            if (gameObject == null) return "";
            return AssetDatabase.TryGetGUIDAndLocalFileIdentifier(Value, out var guid, out long _) ? guid : "";
        }

        private GameObject Deserialize(string str)
        {
            var path = AssetDatabase.GUIDToAssetPath(str);
            return AssetDatabase.LoadAssetAtPath<GameObject>(path);
        }
    }
}