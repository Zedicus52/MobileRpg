using MobileRpg.Interfaces;
using UnityEngine;

namespace MobileRpg.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Attack config", menuName = "Configs/Player Configs/Attack", order = 0)]
    public class PlayerAttackConfig : ScriptableObject
    {
        public WeaponConfig GetWeapon() => _weapon;
        public float GetAttackMultiplier() => _attackMultiplier;
        public float GetBaseAttack() => _baseAttack;
        
        [Range(0.1f, 1.0f)]
        [Tooltip("This multiplier need to calculate max attack amount")] 
        [SerializeField] private float _attackMultiplier;
        [SerializeField] private WeaponConfig _weapon;
        [SerializeField] private float _baseAttack;
        
    }
}