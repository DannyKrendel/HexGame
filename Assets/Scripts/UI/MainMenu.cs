using UnityEngine;
using UnityEngine.UI;

namespace HexGame.UI
{
    public class MainMenu : MenuBase
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButtons;

        protected override MenuType MenuType => MenuType.MainMenu;

        protected override void Awake()
        {
            base.Awake();
            
            _playButton.onClick.AddListener(OnPlayButtonPressed);
            _settingsButtons.onClick.AddListener(OnSettingsButtonPressed);
        }

        private void OnPlayButtonPressed()
        {
            MenuManager.Show(MenuType.LevelMenu);
        }

        private void OnSettingsButtonPressed()
        {
            
        }
    }
}