using UnityEngine;

namespace HexGame.UI
{
    public abstract class MenuBase : MonoBehaviour
    {
        [SerializeField] protected MenuManager MenuManager;

        public bool IsActive => gameObject.activeSelf;
        
        protected abstract MenuType MenuType { get; }
        
        protected virtual void Awake()
        {
            MenuManager.RegisterMenu(MenuType, this);
        }

        public virtual void Show()
        {
            if (IsActive) return;
            
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            if (!IsActive) return;
            
            gameObject.SetActive(false);
        }
    }
}