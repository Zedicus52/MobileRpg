using MobileRpg.Interfaces;
using MobileRpg.ScriptableObjects;

namespace MobileRpg.Models
{
    public class AttackModel : IAttackConfig
    {
        private readonly float _baseDamage;
        private readonly float _attackMultiplier;
        private float _weaponMultiplier;

        public AttackModel(float baseDamage, float attackMultiplier, float weaponMultiplier)
        {
            _baseDamage = baseDamage;
            _attackMultiplier = attackMultiplier;
            _weaponMultiplier = weaponMultiplier;
        }

        public float GetDamage()
        {
            var dmg= (_baseDamage * _attackMultiplier) + _baseDamage;
            return (dmg * _weaponMultiplier) + dmg;
        }

        public void UpdateWeapon(WeaponConfig config) => _weaponMultiplier = config.DamageMultiplier;
    }
}