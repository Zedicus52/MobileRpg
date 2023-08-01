using System;
using MobileRpg.Enums;
using MobileRpg.Monsters;
using MobileRpg.ScriptableObjects;
using UnityEngine;

namespace MobileRpg.Factories.MonsterFactory
{
    public abstract class MonsterFactory : ScriptableObject
    {

        [SerializeField] protected Vector3 _spawnPoint;
        [SerializeField] protected Vector3 _destinationPoint;

        public Monster GetMonster(MonsterType type)
        {
            MonsterConfig config = GetConfig(type);
            if (config == null)
                throw new ArgumentOutOfRangeException($"Config for type {type} not found");
            Monster monster = Instantiate(config.GetPrefab(), _spawnPoint, Quaternion.identity);
            monster.Initialize(config.GetHealth(), config.GetDamage(), config.GetGold(), _destinationPoint, config);
            return monster;
        }
        
        protected abstract MonsterConfig GetConfig(MonsterType type);
        
    }
}