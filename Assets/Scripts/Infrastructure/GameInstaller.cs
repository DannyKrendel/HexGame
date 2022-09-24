﻿using HexGame.Gameplay;
using HexGame.Gameplay.StateMachine;
using HexGame.Input;
using HexGame.UI;
using UnityEngine;
using Zenject;

namespace HexGame.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private HexGrid _hexGrid;
        [SerializeField] private MenuManager _menuManager;
        [SerializeField] private PlayerSpawnPoint _playerSpawnPoint;

        private GameSettings _gameSettings;

        [Inject]
        private void Construct(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }
        
        public override void InstallBindings()
        {
            BindGameCamera();
            BindHexGrid();
            BindGameStateManager();
            BindGameStates();
            BindMenus();
            BindMenuManager();
            BindPlayer();
            BindSpawnPoint();
            BindGridHighlighter();
            BindInputManager();
        }

        private void BindGameCamera()
        {
            Container
                .Bind<GameCamera>()
                .FromComponentInNewPrefab(_gameSettings.GameCameraPrefab)
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

        private void BindPlayer()
        {
            Container
                .BindIFactory<Player>()
                .FromComponentInNewPrefab(_gameSettings.PlayerPrefab)
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
                .Bind<GridHighlighter>()
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
    }
}