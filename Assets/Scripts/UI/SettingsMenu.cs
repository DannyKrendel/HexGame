using UnityEngine;
using UnityEngine.UI;

namespace HexGame.UI
{
    public class SettingsMenu : MenuBase
    {
        [SerializeField] private Button _backButton;
        
        protected override MenuType MenuType => MenuType.SettingsMenu;
        
        protected override void Awake()
        {
            base.Awake();
            
            _backButton.onClick.AddListener(OnBackButtonPressed);
        }
        
        private void OnBackButtonPressed()
        {
            MenuManager.HideCurrent();
        }
    }
}