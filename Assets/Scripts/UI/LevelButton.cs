using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexGame.UI
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text;
        
        private int _levelNumber;
        
        public void Initialize(int number, Action<int> onPressed)
        {
            _levelNumber = number;
            _text.text = number.ToString();
            _button.onClick.AddListener(() => onPressed(number));
        }
    }
}