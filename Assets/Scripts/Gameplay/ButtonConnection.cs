using UnityEngine;

namespace HexGame.Gameplay
{
    public abstract class ButtonConnection : MonoBehaviour
    {
        public abstract void Activate();
        public abstract void Deactivate();
    }
}