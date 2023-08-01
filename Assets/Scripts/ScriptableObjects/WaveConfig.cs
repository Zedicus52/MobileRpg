using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Wave config", menuName = "Configs/Waves Config/Wave", order = 0)]
    public class WaveConfig : ScriptableObject
    {
        [SerializeField] private List<MonsterConfig> _configs;
        [SerializeField] private int _maxMonstersInWave;

        public int GetMaxMonsterCount() => _maxMonstersInWave;
        public List<MonsterConfig> GetMonsterConfigs() => _configs;
    }
}