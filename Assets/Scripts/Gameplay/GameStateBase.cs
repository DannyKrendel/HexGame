using UnityEngine;

namespace HexGame.Gameplay
{
    public abstract class GameStateBase
    {
        public abstract GameStateType GameStateType { get; }
    }
    
    public enum GameStateType
    {
        Gameplay, Pause
    }
}