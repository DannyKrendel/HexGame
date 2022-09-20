using HexGame.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace HexGame.UI
{
    public class LevelMenu : MenuBase
    {
        [SerializeField] private LevelLoader _levelLoader;
        [SerializeField] private GameObject _levelButtonPrefab;
        [SerializeField] private Transform _levelButtonsRoot;
        [SerializeField] private Button _backButton;

        protected override MenuType MenuType => MenuType.LevelMenu;

        protected override void Awake()
        {
            base.Awake();
            
            _backButton.onClick.AddListener(OnBackButtonPressed);
            
            var levelCount = 1;
            for (int i = 1; i <= levelCount; i++)
            {
                var levelButton = Instantiate(_levelButtonPrefab, _levelButtonsRoot).GetComponent<LevelButton>();
                levelButton.Initialize(i, OnLevelButtonPressed);
            }
        }

        private void OnBackButtonPressed()
        {
            MenuManager.HideCurrent();
        }

        private void OnLevelButtonPressed(int number)
        {
            _levelLoader.LoadLevel(number);
        }
    }
}