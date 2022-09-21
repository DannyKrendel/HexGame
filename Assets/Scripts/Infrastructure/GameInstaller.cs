using HexGame.Gameplay;
using UnityEngine;
using Zenject;

namespace HexGame.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameCamera _gameCameraPrefab;
        [SerializeField] private HexGrid _hexGrid;
        
        public override void InstallBindings()
        {
            BindGameCamera();
            BindHexGrid();
            BindGameStateManager();
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
                .Bind<GameStateManager>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .NonLazy();
        }
    }
}