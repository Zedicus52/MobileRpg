using System;
using System.Collections.Generic;
using MobileRpg.Interfaces;
using MobileRpg.Player;
using MobileRpg.ScriptableObjects;
using UnityEngine;

namespace MobileRpg.Core
{
    public class SpellGiver : MonoBehaviour
    {
        public event Action<List<SpellConfig>> ShowSpells;
        [SerializeField] private List<SpellConfig> _spells;
        private PlayerBehaviour _playerBehaviour;
        private IWavesHandler _wavesHandler;

        private void Awake()
        {
            _playerBehaviour = GameBehaviour.Instance.PlayerBehaviour;
            _wavesHandler = GameBehaviour.Instance.WavesHandler;
        }

        private void OnEnable()
        {
            _wavesHandler.NewWaveStarts += OnNewWaveStarts;
        }

        private void OnDisable()
        {
            _wavesHandler.NewWaveStarts -= OnNewWaveStarts;
        }


        private void OnNewWaveStarts(int wave)
        {
            if(wave == 0)
                return;
            
            if (wave % 5 == 0)
            {
                ShowSpells?.Invoke(_spells);
                _wavesHandler.PauseWavesSpawning();
            }
        }

        public void EndInteraction()
        {
            _wavesHandler.ResumeWavesSpawning();
        }

        public void GiveSpell(SpellConfig config)
        {
           _playerBehaviour.PlayerEntity.UpdateSpell(config);
        }
    }
}