using System;
using MobileRpg.Core;
using TMPro;
using UnityEngine;

namespace MobileRpg.UI
{
    public class GoldTextUpdater : MonoBehaviour
    {
        
        [SerializeField] private TMP_Text _goldText;
        private PlayerEntity _playerEntity;

        private void Awake()
        {
            _playerEntity = GameBehaviour.Instance.PlayerBehaviour.PlayerEntity;
        }

        private void OnEnable() => _playerEntity.GoldAmountChanged += OnPlayerGoldChanged;

        private void OnDisable() => _playerEntity.GoldAmountChanged -= OnPlayerGoldChanged;

        private void OnPlayerGoldChanged(int newAmount) => _goldText.text = "Gold: " + newAmount;
    }
}