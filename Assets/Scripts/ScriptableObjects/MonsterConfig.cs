using MobileRpg.Enums;
using MobileRpg.Interfaces;
using MobileRpg.Monsters;
using UnityEngine;

namespace MobileRpg.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Monster Config", menuName = "Configs/Monster Configs/Monster", order = 0)]
    public class MonsterConfig : ScriptableObject, IAttackConfig
    {
        public float GetHealth() => Random.Range(_minHealth, _maxHealth);
        public float GetDamage() => Random.Range(_minDamage, _maxDamage);
        public int GetGold() => Random.Range(_minGoldAmount, _maxGoldAmount);
        public MonsterType GetMonsterType() => _monsterType;
        public Monster GetPrefab() => _prefab;
        
        [Header("Prefab")] 
        [SerializeField] private Monster _prefab;
        
        [Space] [Header("Type")] 
        [SerializeField] private MonsterType _monsterType;
        
        [Space] [Header("Health")]
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _minHealth;

        [Space] [Header("Damage")] 
        [SerializeField] private float _maxDamage;
        [SerializeField] private float _minDamage;

        [Space] [Header("Gold")] 
        [SerializeField] private int _maxGoldAmount;
        [SerializeField] private int _minGoldAmount;
    }
}