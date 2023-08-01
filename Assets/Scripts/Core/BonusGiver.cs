using System;
using System.Collections.Generic;
using MobileRpg.Enums;
using MobileRpg.ScriptableObjects;
using UnityEngine;

namespace MobileRpg.Core
{
    public class BonusGiver : MonoBehaviour
    {
        public event Action<List<BonusConfig>> ShowBonuses;
        [SerializeField] private PlayerBehaviour _playerBehaviour;
        [SerializeField] private List<BonusConfig> _bonuses;
        [SerializeField] private WavesHandler _wavesHandler;

        private void OnEnable()
        {
            _wavesHandler.WavesAreOver += OnWaveAreOver;
        }

        private void OnDisable()
        {
            _wavesHandler.WavesAreOver -= OnWaveAreOver;
        }


        private void OnWaveAreOver()
        {
            ShowBonuses?.Invoke(_bonuses);
        }

        public void EndInteraction()
        {
            
        }

        public void GiveBonus(BonusConfig config)
        {
            PlayerEntity entity = _playerBehaviour.PlayerEntity;
            switch (config.EffectType)
            {
                case BonusEffectType.Heal:
                    entity.RestoreHealth(config.GetBonusAmount(entity.GetCurrentHealth()));
                    break;
                case BonusEffectType.MaxHealth:
                    entity.IncreaseMaxHealth(config.GetBonusAmount(entity.GetCurrentMaxHealth()));
                    break;
                case BonusEffectType.Mana:
                    entity.RestoreMana(config.GetBonusAmount(entity.GetCurrentMana()));
                    break;
                case BonusEffectType.MaxMana:
                    entity.IncreaseMaxMana(config.GetBonusAmount(entity.GetCurrentMaxMana()));
                    break;
                case BonusEffectType.EscapeChance:
                    entity.IncreaseEscapeChance(config.GetBonusAmount(entity.GetCurrentEscapeChance()));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}