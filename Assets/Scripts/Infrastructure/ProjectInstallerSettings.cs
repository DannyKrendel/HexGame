using System;
using PolyternityStuff.SceneManagement;

namespace HexGame.Infrastructure
{
    [Serializable]
    public struct ProjectInstallerSettings
    {
        public string SaveLocation;
        public SceneReference TransitionScene;
    }
}