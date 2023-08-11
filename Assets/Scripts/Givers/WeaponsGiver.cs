using System;
using System.Collections.Generic;
using System.Linq;
using MobileRpg.Core;
using MobileRpg.Player;
using MobileRpg.ScriptableObjects;
using MobileRpg.UI;

namespace MobileRpg.Givers
{
    public class WeaponsGiver : Giver
    {
        private readonly WeaponGiverDisplay _weaponGiverDisplay;
        private readonly WeightRandomList<WeaponConfig> _allWeapons;
        private readonly int _countToGive;

        private WeaponConfig _lastSelectedWeapon;

        public WeaponsGiver(PlayerBehaviour playerBehaviour, WeaponGiverDisplay display, WeightRandomList<WeaponConfig> weapons, int countToGive) : base(playerBehaviour)
        {
            _weaponGiverDisplay = display;
            _allWeapons = weapons;
            _countToGive = countToGive;
            _lastSelectedWeapon = playerBehaviour.PlayerEntity.GetCurrentWeapon();
        }

        public override void StartInteraction()
        {
            _weaponGiverDisplay.ShowWeapons(GetRandomWeapons());
        }

        public override void Subscribe()
        {
            _weaponGiverDisplay.EndedInteraction += OnEndedInteraction;
            _weaponGiverDisplay.GotWeapon += OnGotWeapon;
        }

        public override void UnSubscribe()
        {
            _weaponGiverDisplay.EndedInteraction -= OnEndedInteraction;
            _weaponGiverDisplay.GotWeapon -= OnGotWeapon;
        }
        
        private void OnGotWeapon(WeaponConfig weapon)
        {
            _playerBehaviour.PlayerEntity.UpdateWeapon(weapon);
            _lastSelectedWeapon = weapon;
        }
        
        private List<WeaponConfig> GetRandomWeapons()
        {
            if (_countToGive >= _allWeapons.Count)
                throw new ArgumentException("Need more spells config for generate unique spells");
            
            List<WeaponConfig> configs = new List<WeaponConfig>(_countToGive);
            for (int i = 0; i < _countToGive; i++)
            {
                WeaponConfig config = _allWeapons.GetRandom();
                
                if (config.Id == _lastSelectedWeapon.Id)
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