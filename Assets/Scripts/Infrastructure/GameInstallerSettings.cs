using System;
using UnityEngine;

namespace HexGame.Infrastructure
{
    [Serializable]
    public struct GameInstallerSettings
    {
        public GameObject GameCameraPrefab;
        public GameObject PlayerPrefab;
    }
}