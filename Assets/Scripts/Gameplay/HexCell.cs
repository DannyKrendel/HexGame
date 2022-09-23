using Polyternity;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace HexGame.Gameplay
{
    [ExecuteAlways]
    public class HexCell : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField, ReadOnly] private HexCoordinates _coordinates;
        [SerializeField, HideInInspector] private Grid _grid;

        public HexCoordinates Coordinates => _coordinates;
        
        public Vector2 BoundsSize 
        {
            get
            {
                var worldBoundsSize = _spriteRenderer.bounds.size;
                return new Vector2(worldBoundsSize.x, worldBoundsSize.y) / transform.localScale.x;
            }
        }

        private const float HexRatio = 0.866025404f;
        
        private void Update()
        {
            #if UNITY_EDITOR
            if (!Application.IsPlaying(gameObject))
            {
                if (_grid == null && transform.parent != null && transform.parent.TryGetComponent(out _grid))
                {
                    var coords = _grid.WorldToCell(transform.position);
                    _coordinates = HexCoordinates.FromOffsetCoordinates(coords.x, coords.y);
                    EditorUtility.SetDirty(this);
                    PrefabUtility.RecordPrefabInstancePropertyModifications(this);
                }
                if (_grid != null)
                {
                    transform.position = _grid.CellToWorld(HexCoordinates.ToOffsetCoordinates(_coordinates.X, _coordinates.Z));
                    var scale = _grid.cellSize;
                    scale.x /= HexRatio;
                    transform.localScale = scale;
                }
            }
            #endif
        }
    }
}