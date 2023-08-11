using System;
using System.Collections.Generic;
using MobileRpg.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;


namespace MobileRpg.UI
{
    public class WeaponGiverDisplay : MonoBehaviour
    {
        public event Action EndedInteraction;
        public event Action<WeaponConfig> GotWeapon;
        
        [SerializeField] private Transform _rootPanel;
        [SerializeField] private GridLayoutGroup _weaponContainer;
        [SerializeField] private WeaponDisplay _prefab;
        [SerializeField] private Button _closeButton;
        
        private List<WeaponDisplay> _weapons = new();

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnClose);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnClose);
        }
        public void ShowWeapons(List<WeaponConfig> weapons)
        {
            foreach (WeaponConfig config in weapons)
            {
                WeaponDisplay display = Instantiate(_prefab, Vector3.zero, Quaternion.identity, _weaponContainer.transform);
                display.Initialize(config);
                display.Click += OnWeaponClick;
                _weapons.Add(display);
            }
            _rootPanel.gameObject.SetActive(true);
        }
        private void OnClose()
        {
            _rootPanel.gameObject.SetActive(false);
            foreach (var weapon in _weapons)
            {
                weapon.Click -= OnWeaponClick;
                Destroy(weapon.gameObject);
            }
            EndedInteraction?.Invoke();
            _weapons.Clear();
        }

        private void OnWeaponClick(WeaponConfig config)
        {
            GotWeapon?.Invoke(config);
            OnClose();
        }


        
    }
}