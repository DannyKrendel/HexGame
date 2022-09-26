using HexGame.Gameplay;
using HexGame.Input;
using HexGame.Storage;
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
                .Bind(typeof(IStorage<>)).To(typeof(JsonStorage<>))
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
    }
}
