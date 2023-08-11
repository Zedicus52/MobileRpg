using System;
using System.Collections.Generic;
using MobileRpg.Enums;
using MobileRpg.Player;
using MobileRpg.ScriptableObjects;
using MobileRpg.UI;
using NotImplementedException = System.NotImplementedException;

namespace MobileRpg.Givers
{
    public class BonusesGiver : Giver
    {
        private readonly BonusGiverDisplay _bonusGiverDisplay;
        private readonly List<BonusConfig> _bonuses;

        public BonusesGiver(PlayerBehaviour playerBehaviour, BonusGiverDisplay display, List<BonusConfig> bonuses) : base(playerBehaviour)
        {
            _bonusGiverDisplay = display;
            _bonuses = bonuses;
        }
        

        public override void StartInteraction()
        {
            _bonusGiverDisplay.ShowBonuses(_bonuses);
        }

        public override void Subscribe()
        {
            _bonusGiverDisplay.EndedInteraction += OnEndedInteraction;
            _bonusGiverDisplay.GotBonus += OnGotBonus;
        }

        public override void UnSubscribe()
        {
            _bonusGiverDisplay.EndedInteraction -= OnEndedInteraction;
            _bonusGiverDisplay.GotBonus -= OnGotBonus;
        }

        private void OnGotBonus(BonusConfig config)
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