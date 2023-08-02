using System;
using MobileRpg.Core;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MobileRpg.UI
{
    public class ActionButtonsHolder : MonoBehaviour
    {
        [Header("Buttons")] 
        [SerializeField] private Button _attackButton;
        [SerializeField] private Button _magicAttackButton;
        [SerializeField] private Button _escapeButton;

        private PlayerBehaviour _playerBehaviour;
        private void Awake()
        {
            _playerBehaviour = GameBehaviour.Instance.PlayerBehaviour;
        }

        private void OnEnable()
        {
            _attackButton.onClick.AddListener(_playerBehaviour.Attack);
            _magicAttackButton.onClick.AddListener(_playerBehaviour.MagicAttack);
            _escapeButton.onClick.AddListener(_playerBehaviour.Escape);
        }

        private void OnDisable()
        {
            _attackButton.onClick.RemoveListener(_playerBehaviour.Attack);
            _magicAttackButton.onClick.RemoveListener(_playerBehaviour.MagicAttack);
            _escapeButton.onClick.RemoveListener(_playerBehaviour.Escape);
        }
    }
}