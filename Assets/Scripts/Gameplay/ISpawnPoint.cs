using System;
using UnityEngine;

namespace HexGame.Gameplay
{
    public interface ISpawnPoint<in T> where T : Component
    {
        void Spawn(T actor);
    }
}