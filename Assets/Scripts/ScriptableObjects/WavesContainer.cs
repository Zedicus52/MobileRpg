using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Waves container", menuName = "Configs/Waves Config/Waves Container", order = 0)]
    public class WavesContainer : ScriptableObject
    {
        public int WavesCount => _waveConfigs.Count;
        
        [SerializeField] private List<WaveConfig> _waveConfigs;
        
        
        public WaveConfig GetNextWave(int wave)
        {
            if(wave < WavesCount)
                return _waveConfigs[wave];

            return null;
        }
        
    }
}