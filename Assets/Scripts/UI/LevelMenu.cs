﻿using HexGame.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HexGame.UI
{
    public class LevelMenu : MenuBase
    {
        [SerializeField] private GameObject _levelButtonPrefab;
        [SerializeField] private Transform _levelButtonsRoot;
        [SerializeField] private Button _backButton;

        public override MenuType Type => MenuType.LevelMenu;
        
        private LevelLoader _levelLoader;

        [Inject]
        private void Construct(LevelLoader levelLoader)
        {
            _levelLoader = levelLoader;
        }
        
        private void Awake()
        {
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