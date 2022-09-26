using System.Collections.Generic;
using System.Linq;

namespace HexGame.Gameplay
{
    public class FishManager : HexGridElementManager<Fish>
    {
        public int AllFishCount => Elements.Count;
        public int ConsumedFishCount => Elements.Count(x => x.IsConsumed);
    }
}