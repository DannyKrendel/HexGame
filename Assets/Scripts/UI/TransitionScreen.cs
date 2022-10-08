using Cysharp.Threading.Tasks;
using HexGame.UI.Animation;
using UnityEngine;

namespace HexGame.UI
{
    public class TransitionScreen : MonoBehaviour, IScreen
    {
        [SerializeField] private FadeAnimation _fadeAnimation;
        
        public async UniTask Show()
        {
            gameObject.SetActive(true);
            await _fadeAnimation.PlayFadeIn();
        }

        public async UniTask Hide()
        {
            await _fadeAnimation.PlayFadeOut();
            gameObject.SetActive(false);
        }
    }
}