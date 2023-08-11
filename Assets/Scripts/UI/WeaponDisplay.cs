using System;
using MobileRpg.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.UI
{
    public class WeaponDisplay : MonoBehaviour
    {
        public event Action<WeaponConfig> Click;
        [SerializeField] private Image _spellIcon;
        [SerializeField] private TMP_Text _spellName;
        [SerializeField] private TMP_Text _spellDescription;
        [SerializeField] private Button _mainButton;

        private WeaponConfig _config;

        public void Initialize(WeaponConfig config)
        {
            _config = config;
            _spellIcon.sprite = config.Icon;
            _spellName.text = config.Name;
            _spellDescription.text = config.Description;
            _mainButton.onClick.AddListener(OnSpellClick);
        }
        
        private void OnSpellClick()
        {
            Click?.Invoke(_config);
            _mainButton.onClick.RemoveListener(OnSpellClick);
        }
    }
}