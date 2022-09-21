using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace HexGame.UI
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private MenuType _startMenu;

        private Canvas _rootCanvas;
        private Dictionary<MenuType, MenuBase> _menuDictionary;
        private Stack<MenuType> _menuStack;
        private MenuType? _currentMenu;

        [Inject]
        private void Construct(MenuBase[] menus)
        {
            RegisterMenus(menus);
        }
        
        private void Awake()
        {
            _menuStack = new Stack<MenuType>();
            ShowStartMenu();
        }

        private void RegisterMenus(MenuBase[] menus)
        {
            _menuDictionary = new Dictionary<MenuType, MenuBase>();
            foreach (var menu in menus)
                _menuDictionary.TryAdd(menu.Type, menu);
        }
        
        private void ShowStartMenu()
        {
            _currentMenu = _startMenu;
            _menuStack.Push(_currentMenu.Value);

            foreach (var (menuType, menu) in _menuDictionary)
            {
                if (menuType == _startMenu)
                    menu.Show();
                else
                    menu.Hide();
            }
        }

        public void Show(MenuType menuType)
        {
            if (_menuDictionary.TryGetValue(menuType, out var menu))
            {
                menu.Show();

                if (_currentMenu != null && _menuDictionary.TryGetValue(_currentMenu.Value, out var previousMenu))
                    previousMenu.Hide();

                _currentMenu = menuType;
                _menuStack.Push(_currentMenu.Value);
            }
        }

        public void HideCurrent()
        {
            if (_currentMenu != null && _menuStack.Count > 1 && _menuStack.TryPop(out _) && _menuStack.TryPeek(out var previousMenuType))
            {
                if (_menuDictionary.TryGetValue(_currentMenu.Value, out var currentMenu))
                    currentMenu.Hide();
                
                if (_menuDictionary.TryGetValue(previousMenuType, out var previousMenu))
                    previousMenu.Show();

                _currentMenu = previousMenuType;
            }
        }
    }
    
    public enum MenuType
    {
        MainMenu, LevelMenu, SettingsMenu
    }
}