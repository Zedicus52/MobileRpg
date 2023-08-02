using System.Collections.Generic;
using MobileRpg.Core;
using MobileRpg.Enums;
using MobileRpg.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.UI
{
    public class BonusGiverDisplay : MonoBehaviour
    {
        [SerializeField] private BonusGiver _bonusGiver;
        [SerializeField] private Transform _rootPanel;
        [SerializeField] private Transform _bonusesContainer;
        [SerializeField] private BonusDisplay _prefab;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Color _receivedColor;

        private Dictionary<BonusType, List<BonusDisplay>> _bonuses;
        private int _receivedBonusCount;    

        private void Awake()
        {
            _bonuses = new Dictionary<BonusType, List<BonusDisplay>>
            {
                { BonusType.Health, new List<BonusDisplay>() },
                { BonusType.Mana , new List<BonusDisplay>()},
                { BonusType.Escape, new List<BonusDisplay>()}
            };
        }

        private void OnEnable()
        {
            _bonusGiver.ShowBonuses += OnShowBonuses;
            _closeButton.onClick.AddListener(OnClose);
        }

        private void OnDisable()
        {
            _bonusGiver.ShowBonuses -= OnShowBonuses;
            _closeButton.onClick.RemoveListener(OnClose);
        }

        private void OnShowBonuses(List<BonusConfig> bonuses)
        {
            foreach (BonusConfig bonusConfig in bonuses)
            {
                BonusDisplay display = Instantiate(_prefab, Vector3.zero, Quaternion.identity, _bonusesContainer);
                display.Initialize(bonusConfig);
                display.Click += OnBonusClick;
                AddToDictionary(bonusConfig.BonusType, display);
            }
            _rootPanel.gameObject.SetActive(true);
        }

        private void AddToDictionary(BonusType type, BonusDisplay display)
        {
            if(_bonuses.TryGetValue(type, out var displays))
                displays.Add(display);
        }

        private void OnClose()
        {
            _rootPanel.gameObject.SetActive(false);
            foreach (var bonuse in _bonuses)
            {
                foreach (var bonus in bonuse.Value)
                {
                    bonus.Click -= OnBonusClick;
                    Destroy(bonus.gameObject);
                }
            }
            
            _bonuses.Clear();
            _bonuses = new Dictionary<BonusType, List<BonusDisplay>>
            {
                { BonusType.Health, new List<BonusDisplay>() },
                { BonusType.Mana , new List<BonusDisplay>()},
                { BonusType.Escape, new List<BonusDisplay>()}
            };
            _bonusGiver.EndInteraction();
            _receivedBonusCount = 0;
        }

        private void OnBonusClick(BonusConfig config)
        {
            _bonuses.TryGetValue(config.BonusType, out var bonuses);
            if (bonuses?.Count > 0)
            {
                foreach (var bonusDisplay in bonuses)
                {
                   if(bonusDisplay.Config.Equals(config))
                       bonusDisplay.MarkAsReceived(_receivedColor);
                   else
                       bonusDisplay.DisableInteractable();
                }
            }

            _bonusGiver.GiveBonus(config);
            ++_receivedBonusCount;
            TryToEndInteraction();
        }

        private void TryToEndInteraction()
        {
            if(_receivedBonusCount >= _bonuses.Count)
                OnClose();
        }
        
        
    }
}