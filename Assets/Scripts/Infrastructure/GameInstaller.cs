using HexGame.Gameplay;
using HexGame.Gameplay.StateMachine;
using HexGame.Input;
using HexGame.UI;
using UnityEngine;
using Zenject;

namespace HexGame.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private MenuType _startMenu;
        [SerializeField] private GameStateType _startState;
        [SerializeField] private Grid _gameGrid;
        [SerializeField] private PlayerSpawnPoint _playerSpawnPoint;
        [SerializeField] private LevelFinish _levelFinish;
        [SerializeField] private PlatformManager _platformManager;
        [SerializeField] private FishManager _fishManager;

        private GameInstallerSettings _gameInstallerSettings;

        [Inject]
        private void Construct(GameInstallerSettings gameInstallerSettings)
        {
            _gameInstallerSettings = gameInstallerSettings;
        }
        
        public override void InstallBindings()
        {
            BindGameCamera();
            BindGameGrid();
            BindGridService();
            BindGameStateMachine();
            BindGameStates();
            BindMenus();
            BindMenuManager();
            BindPlayer();
            BindSpawnPoint();
            BindGridHighlighter();
            BindInputManager();
            BindGameplayService();
            BindGridElementManagers();
            BindLevelFinish();
        }

        private void BindGameCamera()
        {
            Container
                .Bind<GameCamera>()
                .FromComponentInNewPrefab(_gameInstallerSettings.GameCameraPrefab)
                .AsSingle()
                .NonLazy();
        }

        private void BindGameGrid()
        {
            Container
                .BindInstance(_gameGrid)
                .AsSingle()
                .NonLazy();
        }

        private void BindGridService()
        {
            Container
                .Bind<GridService>()
                .AsSingle()
                .NonLazy();
        }
        
        private void BindGameStateMachine()
        {
            Container
                .Bind<GameStateMachine>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .NonLazy();
        }
        
        private void BindGameStates()
        {
            Container
                .BindInstance(_startState)
                .WhenInjectedInto<GameStateMachine>()
                .NonLazy();
            
            Container
                .Bind<GameStateBase>().To<GameStateStartLevel>()
                .AsSingle()
                .NonLazy();
            Container
                .Bind<GameStateBase>().To<GameStateGameplay>()
                .AsSingle()
                .NonLazy();
            Container
                .Bind<GameStateBase>().To<GameStatePause>()
                .AsSingle()
                .NonLazy();
            Container
                .Bind<GameStateBase>().To<GameStateWin>()
                .AsSingle()
                .NonLazy();
        }
        
        private void BindMenus()
        {
            Container
                .Bind<MenuBase>()
                .FromComponentsInHierarchy()
                .AsCached()
                .NonLazy();
        }

        private void BindMenuManager()
        {
            Container
                .BindInstance(_startMenu)
                .WhenInjectedInto<MenuManager>()
                .NonLazy();
            Container
                .Bind<MenuManager>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .NonLazy();
        }

        private void BindPlayer()
        {
            Container
                .BindIFactory<Player>()
                .FromComponentInNewPrefab(_gameInstallerSettings.PlayerPrefab)
                .AsSingle()
                .NonLazy();
        }
        
        private void BindSpawnPoint()
        {
            Container
                .Bind<ISpawnPoint<Player>>()
                .FromInstance(_playerSpawnPoint)
                .AsSingle()
                .NonLazy();
        }

        private void BindGridHighlighter()
        {
            Container
                .Bind<PlatformHighlighter>()
                .AsSingle()
                .NonLazy();
        }

        private void BindInputManager()
        {
            Container
                .Bind<InputManager>()
                .AsSingle()
                .NonLazy();
        }

        private void BindGameplayService()
        {
            Container
                .Bind<GameplayService>()
                .AsSingle()
                .NonLazy();
        }
        
        private void BindGridElementManagers()
        {
            Container
                .Bind<PlatformManager>()
                .FromInstance(_platformManager)
                .AsSingle()
                .NonLazy();
            
            Container
                .Bind<FishManager>()
                .FromInstance(_fishManager)
                .AsSingle()
                .NonLazy();
        }
        
        private void BindLevelFinish()
        {
            Container
                .BindInstance(_levelFinish)
                .AsSingle()
                .NonLazy();
        }
    }
}