using HexGame.Gameplay.StateMachine;
using HexGame.UI;
using UnityEngine;
using Zenject;

namespace HexGame.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameCamera _gameCameraPrefab;
        [SerializeField] private HexGrid _hexGrid;
        [SerializeField] private MenuManager _menuManager;
        
        public override void InstallBindings()
        {
            BindGameCamera();
            BindHexGrid();
            BindGameStateManager();
            BindGameStates();
            BindMenus();
            BindMenuManager();
        }

        private void BindGameCamera()
        {
            Container
                .Bind<GameCamera>()
                .FromComponentInNewPrefab(_gameCameraPrefab)
                .AsSingle()
                .NonLazy();
        }

        private void BindHexGrid()
        {
            Container
                .BindInstance(_hexGrid)
                .AsSingle()
                .NonLazy();
        }
        
        private void BindGameStateManager()
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
                .Bind<GameStateBase>().To<GameStateGameplay>()
                .AsSingle()
                .NonLazy();
            Container
                .Bind<GameStateBase>().To<GameStatePause>()
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
                .BindInstance(_menuManager)
                .AsSingle()
                .NonLazy();
        }
    }
}