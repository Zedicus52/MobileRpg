using MobileRpg.Interfaces;
using UnityEngine;

namespace MobileRpg.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Attack config", menuName = "Configs/Player Configs/Attack", order = 0)]
    public class PlayerAttackConfig : ScriptableObject, IAttackConfig
    {
        [Range(0.1f, 1.0f)]
        [Tooltip("This multiplier need to calculate max attack amount")] 
        [SerializeField] private float _attackMultiplier;
        
        [SerializeField] private float _baseAttack;

        public float GetDamage() => (_baseAttack * _attackMultiplier) + _baseAttack;
    }
}