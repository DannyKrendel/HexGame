using Polyternity;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace HexGame.Gameplay
{
    [ExecuteAlways]
    public abstract class HexGridElement : MonoBehaviour, IHexGridElement
    {
        [SerializeField, ReadOnly] private HexCoordinates _coordinates;
        [SerializeField, HideInInspector] private Grid _grid;

        public HexCoordinates Coordinates { get => _coordinates; set => _coordinates = value; }

        private const float HexRatio = 0.866025404f;

        private void Update()
        {
            #if UNITY_EDITOR
            if (Application.IsPlaying(gameObject)) return;

            if (_grid == null)
            {
                SetCoordinates(default);
                
                _grid = transform.GetComponentInParent<Grid>();
                if (_grid == null) return;
                
                var gridService = new GridService(_grid);
                SetCoordinates(gridService.WorldToCoordinates(transform.position));
            }
            if (_grid != null)
            {
                var gridService = new GridService(_grid);
                SetCoordinates(gridService.WorldToCoordinates(transform.position));
                
                var scale = gridService.CellSize;
                scale.x /= HexRatio;
                transform.localScale = scale;
            }
            #endif
        }

        private void SetCoordinates(HexCoordinates coordinates)
        {
            if (_coordinates == coordinates) return;
            _coordinates = coordinates;
            EditorUtility.SetDirty(this);
            PrefabUtility.RecordPrefabInstancePropertyModifications(this);
        }
    }
}