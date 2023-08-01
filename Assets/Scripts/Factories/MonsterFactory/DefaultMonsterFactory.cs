using System.Collections.Generic;
using System.Linq;
using MobileRpg.Enums;
using MobileRpg.ScriptableObjects;
using UnityEngine;

namespace MobileRpg.Factories.MonsterFactory
{
    [CreateAssetMenu (fileName = "Default Monster Factory", menuName = "Factories/Monster/Default")]
    public class DefaultMonsterFactory : MonsterFactory
    {
        [SerializeField] private List<MonsterConfig> _configs;

        protected override MonsterConfig GetConfig(MonsterType type) => 
            _configs.FirstOrDefault(c => c.GetMonsterType().Equals(type));
    }
}