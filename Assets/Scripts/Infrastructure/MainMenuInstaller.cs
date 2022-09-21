using HexGame.Gameplay;
using HexGame.UI;
using UnityEngine;
using Zenject;

namespace HexGame.Infrastructure
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private MenuManager _menuManager;
        [SerializeField] private LevelLoader _levelLoader;
        
        public override void InstallBindings()
        {
            BindMenus();
            BindMenuManager();
            BindLevelLoader();
        }

        private void BindLevelLoader()
        {
            Container
                .BindInstance(_levelLoader)
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