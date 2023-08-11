using System;
using UnityEngine;

namespace MobileRpg.Core
{
    public class Timer : MonoBehaviour
    {
        public event Action TimerFinished;
        public event Action TimerStopped;
        public event Action TimerStarted;
        public event Action<float> TimeUpdated;
        
        [SerializeField] private float _timeRemaining;
        private float _currentTimeRemaining;

        private bool _timerIsStart;
        private bool _timerIsRunning;
        
        
        private void Update()
        {
            if (_timerIsStart == false) return;
            if (_timerIsRunning == false) return;

            if (_currentTimeRemaining > 0)
            {
                _currentTimeRemaining -= Time.deltaTime;
                TimeUpdated?.Invoke(_currentTimeRemaining);
            }
            else
                ResetTimer();
        }

        public void StartTimer()
        {
            _timerIsStart = true;
            _currentTimeRemaining = _timeRemaining;
            _timerIsRunning = true;
            TimerStarted?.Invoke();
        }

        private void ResetTimer()
        {
            StopTimer();
            TimerFinished?.Invoke();
            TimerStarted?.Invoke();
        }

        public void StopTimer()
        {
            _timerIsRunning = false;
            _currentTimeRemaining = 0f;
            _timerIsStart = false;
            TimerStopped?.Invoke();
        }
        
        
    }
}