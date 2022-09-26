using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexGame.Gameplay
{
    public abstract class HexGridElementManager<TElement> : MonoBehaviour
        where TElement : HexGridElement
    {
        protected Dictionary<HexCoordinates, TElement> ElementDictionary;

        public List<TElement> Elements { get; private set; }
        
        protected virtual void Awake()
        {
            Elements = GetComponentsInChildren<TElement>().ToList();
            ElementDictionary = Elements.ToDictionary(x => x.Coordinates);
        }

        public bool TryGetElement(HexCoordinates coordinates, out TElement element)
        {
            return ElementDictionary.TryGetValue(coordinates, out element);
        }
    }
}