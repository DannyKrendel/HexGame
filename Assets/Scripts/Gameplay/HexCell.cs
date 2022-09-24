using System;
using Polyternity;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace HexGame.Gameplay
{
    [ExecuteAlways]
    public class HexCell : MonoBehaviour, IHighlight
    {
        [SerializeField, ReadOnly] private HexCoordinates _coordinates;
        [SerializeField, Range(0, 10)] private int _startDurability = 1;
        [SerializeField] private GameObject _selectGameObject;
        [SerializeField, HideInInspector] private Grid _grid;

        public HexCoordinates Coordinates => _coordinates;
        public bool IsHighlighted { get; private set; }
        public int Durability { get; private set; }

        private const float HexRatio = 0.866025404f;

        private void Awake()
        {
            Durability = _startDurability;
        }

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
        
        public void Highlight()
        {
            if (IsHighlighted) return;
            _selectGameObject.SetActive(true);
            IsHighlighted = true;
        }
        
        public void ClearHighlight()
        {
            if (!IsHighlighted) return;
            _selectGameObject.SetActive(false);
            IsHighlighted = false;
        }

        public void SubtractDurability(int amount = 1)
        {
            Durability = Mathf.Max(Durability - amount, 0);
            if (Durability == 0)
                Break();
        }

        private void Break()
        {
            gameObject.SetActive(false);
        }
    }
}