using HexGame.Gameplay;
using UnityEngine;
using Zenject;

namespace HexGame.Infrastructure
{
    [CreateAssetMenu(menuName = "Game Settings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        [SerializeField] private ProjectInstallerSettings _projectInstallerSettings;
        [SerializeField] private GameInstallerSettings _gameInstallerSettings;
        [SerializeField] private GameSettings _gameSettings;

        public override void InstallBindings()
        {
            BindProjectInstallerSettings();
            BindGameInstallerSettings();
            BindGameSettings();
        }

        private void BindProjectInstallerSettings()
        {
            Container
                .BindInstance(_projectInstallerSettings)
                .AsSingle()
                .NonLazy();
        }

        private void BindGameInstallerSettings()
        {
            Container
                .BindInstance(_gameInstallerSettings)
                .AsSingle()
                .NonLazy();
        }

        private void BindGameSettings()
        {
            Container
                .BindInstance(_gameSettings)
                .AsSingle()
                .NonLazy();
        }
    }
}