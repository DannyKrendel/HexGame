using UnityEngine;
using UnityEngine.UI;

namespace HexGame.UI
{
    public class SettingsMenu : MenuBase
    {
        [SerializeField] private Button _backButton;
        
        public override MenuType Type => MenuType.SettingsMenu;
        
        private void Awake()
        {
            _backButton.onClick.AddListener(OnBackButtonPressed);
        }
        
        private void OnBackButtonPressed()
        {
            MenuManager.HideCurrent();
        }
    }
}