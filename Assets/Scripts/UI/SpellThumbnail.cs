using System;
using MobileRpg.Core;
using MobileRpg.Player;
using MobileRpg.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.UI
{
    public class SpellThumbnail : MonoBehaviour
    {
        public event Action<SpellConfig> SpellThumbnailClicked;
        
        [SerializeField] private Image _spellIcon;
        [SerializeField] private Button _spellButton;

        private SpellConfig _spellConfig;

        private PlayerEntity PlayerEntity => GameBehaviour.Instance.PlayerBehaviour.PlayerEntity;

        private void Awake()
        {
            UpdateSpell(PlayerEntity.GetCurrentSpell());
        }

        private void UpdateSpell(SpellConfig spellConfig)
        {
            _spellConfig = spellConfig;
            _spellIcon.sprite = _spellConfig.Icon;
        }

        private void OnEnable()
        {
            PlayerEntity.SpellChanged += UpdateSpell;
            _spellButton.onClick.AddListener(OnButtonClick);
        }
        
        private void OnDisable()
        {
            _spellButton.onClick.RemoveListener(OnButtonClick);
            PlayerEntity.SpellChanged -= UpdateSpell;
        }

        private void OnButtonClick()
        {
            Debug.Log(_spellConfig);
            SpellThumbnailClicked?.Invoke(_spellConfig);
        }
    }
}