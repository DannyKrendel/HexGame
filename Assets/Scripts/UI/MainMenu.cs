using UnityEngine;
using UnityEngine.UI;

namespace HexGame.UI
{
    public class MainMenu : MenuBase
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButtons;

        public override MenuType Type => MenuType.MainMenu;

        private void Awake()
        {
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