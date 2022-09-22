namespace HexGame.Gameplay.StateMachine
{
    public abstract class GameStateBase : IState
    {
        protected readonly GameStateMachine GameStateMachine;

        public abstract GameStateType Type { get; }
        
        protected GameStateBase(GameStateMachine gameStateMachine)
        {
            GameStateMachine = gameStateMachine;
        }

        public virtual void Enter() { }

        public virtual void Exit() { }

        public virtual void Update() { }

        public virtual void FixedUpdate() { }
    }
}