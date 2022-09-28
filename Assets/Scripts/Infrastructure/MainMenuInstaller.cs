using HexGame.UI;
using UnityEngine;
using Zenject;

namespace HexGame.Infrastructure
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private MenuType _startMenu;

        public override void InstallBindings()
        {
            BindMenus();
            BindMenuManager();
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
    }
}