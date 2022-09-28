using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexGame.Gameplay
{
    public class HexGridElementManager<TElement> where TElement : HexGridElement
    {
        protected readonly Dictionary<HexCoordinates, TElement> ElementDictionary;

        public List<TElement> Elements { get; }

        protected HexGridElementManager(Grid grid)
        {
            Elements = grid.GetComponentsInChildren<TElement>().ToList();
            ElementDictionary = Elements.ToDictionary(x => x.Coordinates);
        }

        public bool TryGetElement(HexCoordinates coordinates, out TElement element)
        {
            return ElementDictionary.TryGetValue(coordinates, out element);
        }
    }
}