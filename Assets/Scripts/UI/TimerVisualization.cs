using System;
using MobileRpg.Core;
using MobileRpg.Player;
using TMPro;
using UnityEngine;

namespace MobileRpg.UI
{
    public class TimerVisualization : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timerText;

        private PlayerBehaviour _playerBehaviour => GameBehaviour.Instance.PlayerBehaviour;

        private void OnEnable()
        {
            _playerBehaviour.Timer.TimeUpdated += OnTimerUpdated;
            _playerBehaviour.Timer.TimerFinished += OnTimerFinished;
            _playerBehaviour.Timer.TimerStarted += OnTimerStarted;
            _playerBehaviour.Timer.TimerStopped += OnTimerFinished;
        }
        
        private void OnDisable()
        {
            _playerBehaviour.Timer.TimeUpdated -= OnTimerUpdated;
            _playerBehaviour.Timer.TimerFinished -= OnTimerFinished;
            _playerBehaviour.Timer.TimerStarted -= OnTimerStarted;
            _playerBehaviour.Timer.TimerStopped -= OnTimerFinished;
        }
        
        private void OnTimerFinished()
        {
            _timerText.gameObject.SetActive(false);
        }
        
        private void OnTimerStarted()
        {
            _timerText.gameObject.SetActive(true);
        }

        private void OnTimerUpdated(float timeToDisplay)
        {
            if(timeToDisplay < 0)
                return;
            
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);  
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            _timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}