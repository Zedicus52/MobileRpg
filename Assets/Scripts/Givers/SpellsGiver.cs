using System;
using System.Collections.Generic;
using System.Linq;
using MobileRpg.Core;
using MobileRpg.Player;
using MobileRpg.ScriptableObjects;
using MobileRpg.UI;


namespace MobileRpg.Givers
{
    public class SpellsGiver : Giver
    {
        private readonly SpellGiverDisplay _spellGiverDisplay;
        private readonly WeightRandomList<SpellConfig> _spells;
        private readonly int _countToGive;

        private SpellConfig _lastSelectedSpell;

        public SpellsGiver(PlayerBehaviour playerBehaviour, SpellGiverDisplay display, 
            WeightRandomList<SpellConfig> spells, int countToGive) : base(playerBehaviour)
        {
            _spellGiverDisplay = display;
            _spells = spells;
            _countToGive = countToGive;
            _lastSelectedSpell = playerBehaviour.PlayerEntity.GetCurrentSpell();
        }

        public override void StartInteraction()
        {
            _spellGiverDisplay.ShowSpells(GetRandomSpells());
        }

        public override void Subscribe()
        {
            _spellGiverDisplay.EndedInteraction += OnEndedInteraction;
            _spellGiverDisplay.GotSpell += OnGotSpell;
        }
        
        public override void UnSubscribe()
        {
            _spellGiverDisplay.EndedInteraction -= OnEndedInteraction;
            _spellGiverDisplay.GotSpell -= OnGotSpell;
        }
        
        private void OnGotSpell(SpellConfig spell)
        {
            _playerBehaviour.PlayerEntity.UpdateSpell(spell);
            _lastSelectedSpell = spell;
        }
        
        private List<SpellConfig> GetRandomSpells()
        {
            if (_countToGive >= _spells.Count)
                throw new ArgumentException("Need more spells config for generate unique spells");
            
            List<SpellConfig> configs = new List<SpellConfig>(_countToGive);
            for (int i = 0; i < _countToGive; i++)
            {
                SpellConfig config = _spells.GetRandom();
                
                if (config.Id == _lastSelectedSpell.Id)
                {
                    --i;
                    continue;
                }

                if (configs.Any(x => x.Id == config.Id))
                {
                    --i;
                    continue;
                }

                configs.Add(config);
                
            }
            return configs;
        }
    }
}