using System;
using System.Collections.Generic;
using System.Linq;
using MobileRpg.Enums;
using MobileRpg.Monsters;
using MobileRpg.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MobileRpg.Factories.MonsterFactory
{
    
    [CreateAssetMenu (fileName = "Configured Monster Factory", menuName = "Factories/Monster/Configured")]
    public class ConfiguredMonsterFactory : MonsterFactory
    {
        private List<MonsterConfig> _configs;
        
        public Monster GetMonster() => GetMonster(GetRandomType());

        public void UpdateMonsterConfigs(List<MonsterConfig> configs) => 
            _configs = new List<MonsterConfig>(configs);

        protected override MonsterConfig GetConfig(MonsterType type) => 
            _configs.FirstOrDefault(x => x.GetMonsterType().Equals(type));

        private MonsterType GetRandomType()
        {
            int index = Random.Range(0, _configs.Count);
            return _configs[index].GetMonsterType();
        }
            
    }
}