using System;
using MobileRpg.Core;
using MobileRpg.Player;
using MobileRpg.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.UI
{
    public class WeaponThumbnail : MonoBehaviour
    {
        
        [SerializeField] private Image _weaponIcon;
        [SerializeField] private Button _weaponButton;

        private WeaponConfig _weaponConfig;

        private PlayerEntity PlayerEntity => GameBehaviour.Instance.PlayerBehaviour.PlayerEntity;

        private void Awake()
        {
            UpdateWeapon(PlayerEntity.GetCurrentWeapon());
        }

        private void UpdateWeapon(WeaponConfig spellConfig)
        {
            _weaponConfig = spellConfig;
            _weaponIcon.sprite = _weaponConfig.Icon;
        }

        private void OnEnable()
        {
            PlayerEntity.WeaponChanged += UpdateWeapon;
            _weaponButton.onClick.AddListener(OnButtonClick);
        }
        
        private void OnDisable()
        {
            _weaponButton.onClick.RemoveListener(OnButtonClick);
            PlayerEntity.WeaponChanged -= UpdateWeapon;
        }

        private void OnButtonClick()
        {
            Debug.Log(_weaponConfig);
        }
    }
}