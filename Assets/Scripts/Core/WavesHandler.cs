using System;
using MobileRpg.Factories.MonsterFactory;
using MobileRpg.Monsters;
using MobileRpg.ScriptableObjects;
using UnityEngine;

namespace MobileRpg.Core
{
    public class WavesHandler : MonoBehaviour
    {
        public event Action WavesAreOver;
        public event Action<int> NewWaveStarts;

        [SerializeField] private WavesContainer _wavesContainer;
        [SerializeField] private ConfiguredMonsterFactory _monsterFactory;
        
        private WaveConfig _currentWave;
        private int _currentWaveIndex;
        private int _currentMonsterCount;
        private bool _canSpawn;

        private void Awake()
        {
            StartWavesSpawning();
        }

        public void StartWavesSpawning()
        {
            _currentWave = _wavesContainer.GetNextWave(_currentWaveIndex++);
            _monsterFactory.UpdateMonsterConfigs(_currentWave.GetMonsterConfigs());
        }

        public void PauseWavesSpawning() => _canSpawn = true;

        public void ResumeWavesSpawning() => _canSpawn = false;

        public void StopWavesSpawning()
        {
            
        }

        public Monster GetNextMonster()
        {
            if (_canSpawn)
                return null;
            
            TryUpdateWave();

            
            ++_currentMonsterCount;
            return _monsterFactory.GetMonster();

        }

        private void TryUpdateWave() 
        {
            if (_currentMonsterCount != _currentWave.GetMaxMonsterCount()) 
                return;
            
            _currentWave = _wavesContainer.GetNextWave(_currentWaveIndex++);
            if (_currentWave == null)
            {
                WavesAreOver?.Invoke();
                return;
            }
            _monsterFactory.UpdateMonsterConfigs(_currentWave.GetMonsterConfigs());
            _currentMonsterCount = 0;
            NewWaveStarts?.Invoke(_currentWaveIndex+1);
        }

    }
}