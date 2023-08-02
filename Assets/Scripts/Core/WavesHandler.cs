using System;
using MobileRpg.Factories.MonsterFactory;
using MobileRpg.Interfaces;
using MobileRpg.Monsters;
using MobileRpg.ScriptableObjects;
using UnityEngine;

namespace MobileRpg.Core
{
    public class WavesHandler : IWavesHandler
    {
        public event Action WavesAreOver;
        public event Action WavesSpawningStarted;
        public event Action WavesSpawningResume;
        public event Action<int> NewWaveStarts;

        private readonly WavesContainer _wavesContainer;
        private readonly ConfiguredMonsterFactory _monsterFactory;
        
        private WaveConfig _currentWave;
        private int _currentWaveIndex;
        private int _currentMonsterCount;
        private bool _canSpawn;

        public WavesHandler(WavesContainer container, ConfiguredMonsterFactory monsterFactory)
        {
            _wavesContainer = container;
            _monsterFactory = monsterFactory;
        }

        
        public void StartWavesSpawning()
        {
            _canSpawn = true;
  
            _currentWave = _wavesContainer.GetNextWave(_currentWaveIndex++);
            _monsterFactory.UpdateMonsterConfigs(_currentWave.GetMonsterConfigs());
            WavesSpawningStarted?.Invoke();
            //NewWaveStarts?.Invoke(_currentWaveIndex-1);
        }

        public void PauseWavesSpawning() => _canSpawn = false;

        public void ResumeWavesSpawning()
        {
            _canSpawn = true;
            WavesSpawningResume?.Invoke();
        }

        public void StopWavesSpawning()
        {
            _canSpawn = false;
            Reset();
        }

        private void Reset()
        {
            _currentWaveIndex = 0;
            _currentMonsterCount = 0;
        }

        public Monster GetNextMonster()
        {
            TryUpdateWave();

            if (_canSpawn == false)
                return null;
                
            
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
                StopWavesSpawning();
                return;
            }
            
            _monsterFactory.UpdateMonsterConfigs(_currentWave.GetMonsterConfigs());
            _currentMonsterCount = 0;
            NewWaveStarts?.Invoke(_currentWaveIndex-1);
        }

    }
}