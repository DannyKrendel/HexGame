using HexGame.Input;
using Zenject;

namespace HexGame.Infrastructure
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGameInput();
        }

        private void BindGameInput()
        {
            Container
                .Bind<GameInput>()
                .AsSingle()
                .NonLazy();
        }
    }
}
