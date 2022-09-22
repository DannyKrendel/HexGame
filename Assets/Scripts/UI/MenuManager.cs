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
        private Stack<MenuBase> _menuStack;
        private MenuBase _currentMenu;

        [Inject]
        private void Construct(MenuBase[] menus)
        {
            RegisterMenus(menus);
        }
        
        private void Awake()
        {
            _menuStack = new Stack<MenuBase>();
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
            if (_menuDictionary.TryGetValue(_startMenu, out _currentMenu))
                _menuStack.Push(_currentMenu);

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
                if (_currentMenu != null)
                    _currentMenu.Hide();

                _currentMenu = menu;
                _currentMenu.Show();
                _menuStack.Push(_currentMenu);
            }
        }

        public void HideCurrent()
        {
            if (_currentMenu != null && _menuStack.TryPop(out _))
            {
                _currentMenu.Hide();
                
                if (_menuStack.TryPeek(out var previousMenu))
                {
                    _currentMenu = previousMenu;
                    _currentMenu.Show();
                }
            }
        }
    }
    
    public enum MenuType
    {
        None, MainMenu, LevelMenu, SettingsMenu, PauseMenu
    }
}