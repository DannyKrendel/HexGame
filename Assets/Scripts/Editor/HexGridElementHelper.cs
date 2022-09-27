using System.Collections.Generic;
using System.Linq;
using HexGame.Gameplay;
using Polyternity.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HexGame.Editor
{
    [InitializeOnLoad]
    public static class HexGridElementHelper
    {
        private static readonly List<ElementData> Elements;
        private const float HexRatio = 0.866025404f;
        
        private static bool _isMouseInScene;
        
        static HexGridElementHelper()
        {
            Elements = new List<ElementData>();
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
            SceneView.duringSceneGui += OnSceneGui;
        }
        
        private static void OnHierarchyChanged()
        {
            foreach (var gameObject in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                var grids = gameObject.GetComponentsInChildren<Grid>();

                foreach (var grid in grids)
                {
                    foreach (var element in grid.GetComponentsInChildren<HexGridElement>())
                    {
                        if (Elements.Exists(x => x.Element == element)) continue;
                        
                        var newElement = new ElementData
                        {
                            Grid = grid,
                            Element = element
                        };
                        SetupNewElement(newElement);
                        Elements.Add(newElement);
                    }
                }
            }
        }
        
        private static void OnSceneGui(SceneView sceneView)
        {
            if (Event.current.type == EventType.MouseEnterWindow)
                _isMouseInScene = true;
            if (Event.current.type == EventType.MouseLeaveWindow)
                _isMouseInScene = false;
            
            if (Elements.Count == 0 || Event.current.type != EventType.Layout) return;
            
            foreach (var elementData in Elements)
            {
                if (elementData.Element == null || elementData.Grid == null) 
                    continue;
                UpdateElement(elementData, _isMouseInScene);
            }
        }

        private static void SetupNewElement(ElementData elementData)
        {
            var serializedObject = new SerializedObject(elementData.Element);
            var coordinatesProp = serializedObject.FindProperty("_coordinates");
            var gridService = new GridService(elementData.Grid);
            
            var newCoordinates = gridService.WorldToCoordinates(elementData.Element.transform.position);
            
            if (newCoordinates != elementData.Element.Coordinates)
                coordinatesProp.SetValue(newCoordinates);
        }

        private static void UpdateElement(ElementData elementData, bool snap = false)
        {
            var gridService = new GridService(elementData.Grid);
            
            if (snap)
            {
                var serializedObject = new SerializedObject(elementData.Element);
                var coordinatesProp = serializedObject.FindProperty("_coordinates");
                
                SnapToGrid(elementData);
                
                var newCoordinates = gridService.WorldToCoordinates(elementData.Element.transform.position);
            
                if (newCoordinates != elementData.Element.Coordinates)
                    coordinatesProp.SetValue(newCoordinates);
            }
            else
            {
                elementData.Element.transform.position = gridService.CoordinatesToWorld(elementData.Element.Coordinates);
                UpdateLocalScale(elementData);
            }
        }

        private static void UpdateLocalScale(ElementData elementData)
        {
            var gridService = new GridService(elementData.Grid);
            var scale = gridService.CellSize;
            scale.x /= HexRatio;
            
            if (elementData.Element.transform.localScale == scale) return;
            
            elementData.Element.transform.localScale = scale;
        }

        private static void SnapToGrid(ElementData elementData)
        {
            var transform = elementData.Element.transform;
            var cellPos = elementData.Grid.WorldToCell(transform.position);
            transform.position = elementData.Grid.CellToWorld(cellPos);
        }

        private struct ElementData
        {
            public Grid Grid;
            public HexGridElement Element;
        }
    }
}