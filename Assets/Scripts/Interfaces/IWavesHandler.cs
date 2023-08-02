using System;
using MobileRpg.Monsters;

namespace MobileRpg.Interfaces
{
    public interface IWavesHandler
    {
        public event Action WavesAreOver;
        public event Action<int> NewWaveStarts;
        public event Action WavesSpawningStarted;
        public event Action WavesSpawningResume;

        void StartWavesSpawning();
        void PauseWavesSpawning();
        void ResumeWavesSpawning();
        void StopWavesSpawning();
        Monster GetNextMonster();

    }
}