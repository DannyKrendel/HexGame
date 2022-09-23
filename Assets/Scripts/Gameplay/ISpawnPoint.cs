using System;
using UnityEngine;

namespace HexGame.Gameplay
{
    public interface ISpawnPoint<out T> where T : Component
    {
        event Action<T> Spawned;

        T Spawn();
    }
}