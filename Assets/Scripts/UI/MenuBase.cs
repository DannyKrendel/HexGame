using UnityEngine;
using Zenject;

namespace HexGame.UI
{
    public abstract class MenuBase : MonoBehaviour
    {
        public bool IsActive => gameObject.activeSelf;
        
        public abstract MenuType Type { get; }

        protected MenuManager MenuManager;
        
        [Inject]
        private void Construct(MenuManager menuManager)
        {
            MenuManager = menuManager;
        }

        public virtual void Show()
        {
            if (!IsActive)
                gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            if (IsActive)
                gameObject.SetActive(false);
        }
    }
}