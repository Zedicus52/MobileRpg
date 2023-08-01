using MobileRpg.Core;
using TMPro;
using UnityEngine;

namespace MobileRpg.UI
{
    public class GoldTextUpdater : MonoBehaviour
    {
        [SerializeField] private PlayerBehaviour _playerBehaviour;
        [SerializeField] private TMP_Text _goldText;

        private void OnEnable() => _playerBehaviour.PlayerEntity.GoldAmountChanged += OnPlayerGoldChanged;

        private void OnDisable() => _playerBehaviour.PlayerEntity.GoldAmountChanged -= OnPlayerGoldChanged;

        private void OnPlayerGoldChanged(int newAmount) => _goldText.text = "Gold: " + newAmount;
    }
}