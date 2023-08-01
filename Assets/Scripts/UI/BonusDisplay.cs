using System;
using MobileRpg.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.UI
{
    public class BonusDisplay : MonoBehaviour
    {
        public event Action<BonusConfig> Click;


        public BonusConfig Config => _bonusConfig;
        [SerializeField] private Image _bonusIcon;
        [SerializeField] private TMP_Text _bonusName;
        [SerializeField] private TMP_Text _bonusDescription;
        [SerializeField] private Button _mainButton;
        
        private BonusConfig _bonusConfig;

        public void Initialize(BonusConfig bonusConfig)
        {
            _bonusConfig = bonusConfig;
            _bonusIcon.sprite = _bonusConfig.BonusIcon;
            _bonusName.text = _bonusConfig.BonusName;
            _bonusDescription.text = _bonusConfig.BonusDescription;
            _mainButton.onClick.AddListener(OnBonusClick);
        }
        
        private void OnBonusClick()
        {
            Click?.Invoke(_bonusConfig);
            _mainButton.onClick.RemoveListener(OnBonusClick);
        }

        public void DisableInteractable() => _mainButton.interactable = false;
        public void MarkAsReceived(Color color) => _mainButton.image.color = color;

    }
}