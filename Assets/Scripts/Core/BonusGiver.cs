using System;
using System.Collections.Generic;
using MobileRpg.Enums;
using MobileRpg.Interfaces;
using MobileRpg.ScriptableObjects;
using UnityEngine;

namespace MobileRpg.Core
{
    public class BonusGiver : MonoBehaviour
    {
        public event Action<List<BonusConfig>> ShowBonuses;
        [SerializeField] private List<BonusConfig> _bonuses;
        private PlayerBehaviour _playerBehaviour;
        private IWavesHandler _wavesHandler;

        private void Awake()
        {
            _playerBehaviour = GameBehaviour.Instance.PlayerBehaviour;
            _wavesHandler = GameBehaviour.Instance.WavesHandler;
        }

        private void OnEnable()
        {
            _wavesHandler.NewWaveStarts += OnNewWaveStarts;
        }

        private void OnDisable()
        {
            _wavesHandler.NewWaveStarts -= OnNewWaveStarts;
        }


        private void OnNewWaveStarts(int wave)
        {
            Debug.Log("Wave: "+ wave);
            if(wave == 0)
                return;
            
            if (wave % 3 == 0)
            {
                
                ShowBonuses?.Invoke(_bonuses);
                _wavesHandler.PauseWavesSpawning();
            }
                
        }

        public void EndInteraction()
        {
            _wavesHandler.ResumeWavesSpawning();
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