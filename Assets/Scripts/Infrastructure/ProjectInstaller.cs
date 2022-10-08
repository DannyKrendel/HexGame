using HexGame.Gameplay;
using HexGame.Input;
using HexGame.Storage;
using HexGame.UI;
using Zenject;

namespace HexGame.Infrastructure
{
    public class ProjectInstaller : MonoInstaller
    {
        private ProjectInstallerSettings _projectInstallerSettings;
        
        [Inject]
        private void Construct(ProjectInstallerSettings projectInstallerSettings)
        {
            _projectInstallerSettings = projectInstallerSettings;
        }
        
        public override void InstallBindings()
        {
            BindGameInput();
            BindStorage();
            BindSaveSystem();
            BindSaveManager();
            BindLevelManager();
            BindTransitionEventBus();
            BindTransitionManager();
        }

        private void BindGameInput()
        {
            Container
                .Bind<GameInput>()
                .AsSingle()
                .NonLazy();
        }

        private void BindStorage()
        {
            Container
                .Bind(typeof(IStorage<>)).To(typeof(BinaryStorage<>))
                .AsSingle();
        }

        private void BindSaveSystem()
        {
            Container
                .BindInstance(_projectInstallerSettings.SaveLocation)
                .WhenInjectedInto<SaveSystem.SaveSystem>()
                .NonLazy();
            Container
                .Bind<SaveSystem.SaveSystem>()
                .AsSingle()
                .NonLazy();
        }

        private void BindSaveManager()
        {
            Container
                .Bind<SaveManager>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .NonLazy();
        }
        
        private void BindLevelManager()
        {
            Container
                .Bind<LevelManager>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .NonLazy();
        }

        private void BindTransitionEventBus()
        {
            Container
                .Bind<TransitionEventBus>()
                .AsSingle()
                .NonLazy();
        }

        private void BindTransitionManager()
        {
            Container
                .BindInstance(_projectInstallerSettings.TransitionScene)
                .WhenInjectedInto<TransitionManager>()
                .NonLazy();
            Container
                .Bind<TransitionManager>()
                .AsSingle()
                .NonLazy();
        }
    }
}
