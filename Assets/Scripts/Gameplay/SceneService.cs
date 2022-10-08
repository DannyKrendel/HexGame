using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace HexGame.Gameplay
{
    public class SceneService
    {
        private readonly TransitionEventBus _transitionEventBus;

        public SceneService(TransitionEventBus transitionEventBus)
        {
            _transitionEventBus = transitionEventBus;
        }

        public async UniTask SwitchSceneAsync(int buildIndex, bool withTransition = false)
        {
            if (withTransition)
                await _transitionEventBus.Raise(TransitionEventBus.EventType.BeforeAction);
            await SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
            await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            if (withTransition)
                await _transitionEventBus.Raise(TransitionEventBus.EventType.AfterAction);
        }
    }
}