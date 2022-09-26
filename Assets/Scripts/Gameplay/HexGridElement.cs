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

        public HexCoordinates Coordinates => _coordinates;

        private const float HexRatio = 0.866025404f;

        private void Update()
        {
            #if UNITY_EDITOR
            if (Application.IsPlaying(gameObject)) return;

            if (_grid == null)
            {
                _grid = transform.GetComponentInParent<Grid>();
                if (_grid == null) return;
                
                var gridService = new GridService(_grid);
                _coordinates = gridService.WorldToCoordinates(transform.position);
                EditorUtility.SetDirty(this);
                PrefabUtility.RecordPrefabInstancePropertyModifications(this);
            }
            if (_grid != null)
            {
                var gridService = new GridService(_grid);
                transform.position = gridService.CoordinatesToWorld(_coordinates);
                var scale = gridService.CellSize;
                scale.x /= HexRatio;
                transform.localScale = scale;
            }
            #endif
        }
    }
}