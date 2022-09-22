namespace HexGame.Gameplay.StateMachine
{
    public class GameStatePause : GameStateBase
    {
        public override GameStateType Type => GameStateType.Pause;

        public GameStatePause(GameStateMachine gameStateMachine) : base(gameStateMachine)
        {
            
        }
    }
}