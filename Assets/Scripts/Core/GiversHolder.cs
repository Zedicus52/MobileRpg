using System;
using System.Collections.Generic;
using System.Linq;
using MobileRpg.Givers;
using MobileRpg.Interfaces;
using MobileRpg.Player;
using MobileRpg.ScriptableObjects;
using MobileRpg.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace MobileRpg.Core
{
    public class GiversHolder : MonoBehaviour
    {
        [Header("Bonuses giver config")]
        [SerializeField] private BonusGiverDisplay _bonusGiverDisplay;
        [SerializeField] private List<BonusConfig> _bonusConfigs;
        [SerializeField] private int _bonusesShowFrequency = 3;

        [Header("Spells giver config"), Space] 
        [SerializeField] private SpellGiverDisplay _spellsGiverDisplay;
        [SerializeField] private WeightRandomList<SpellConfig> _spellConfigs;
        [SerializeField] private int _spellsShowFrequency = 5;
        [SerializeField] private int _spellsCountToGive = 3;

        [Header("Weapons giver config"), Space] 
        [SerializeField] private WeaponGiverDisplay _weaponGiverDisplay;
        [SerializeField] private WeightRandomList<WeaponConfig> _weaponsConfigs;
        [SerializeField] private int _weaponsShowFrequency = 5;
        [SerializeField] private int _weaponsCountToGive = 3;

        private PlayerBehaviour _playerBehaviour;
        private IWavesHandler _wavesHandler;

        private List<Giver> _allGivers;
        private Queue<Giver> _giversToShow;

        private void Awake()
        {
            _playerBehaviour = GameBehaviour.Instance.PlayerBehaviour;
            _wavesHandler = GameBehaviour.Instance.WavesHandler;
            _allGivers = new List<Giver>()
            {
                new BonusesGiver(_playerBehaviour, _bonusGiverDisplay, _bonusConfigs),
                new SpellsGiver(_playerBehaviour, _spellsGiverDisplay, _spellConfigs, _spellsCountToGive),
                new WeaponsGiver(_playerBehaviour, _weaponGiverDisplay, _weaponsConfigs, _weaponsCountToGive)
            };
            _giversToShow = new Queue<Giver>();
        }
        
        private void OnEnable()
        {
            _wavesHandler.NewWaveStarts += OnNewWaveStarts;
            foreach (var giver in _allGivers)
            {
                giver.Subscribe();
                giver.EndedInteraction += OnGiverEndInteraction;
            }
        }
        
        private void OnDisable()
        {
            _wavesHandler.NewWaveStarts -= OnNewWaveStarts;
            foreach (var giver in _allGivers)
            {
                giver.UnSubscribe();
                giver.EndedInteraction -= OnGiverEndInteraction;
            }
        }
        
        private void OnNewWaveStarts(int wave)
        {
            if(wave == 0)
                return;

            if (wave % _bonusesShowFrequency == 0)
                _giversToShow.Enqueue(GetGiver<BonusesGiver>());
            if(wave % _spellsShowFrequency == 0)
                _giversToShow.Enqueue(GetGiver<SpellsGiver>());
            if(wave % _weaponsShowFrequency == 0)
                _giversToShow.Enqueue(GetGiver<WeaponsGiver>());


            if (_giversToShow.Count > 0)
            {
                _wavesHandler.PauseWavesSpawning();
                _giversToShow.Dequeue().StartInteraction();
            }
                
        }
        
        private void OnGiverEndInteraction()
        {
            if (_giversToShow.Count > 0)
            {
                _giversToShow.Dequeue().StartInteraction();
                return;
            }
            
            _wavesHandler.ResumeWavesSpawning();
                
        }

        private Giver GetGiver<T>() where T : Giver
        {
            Giver giver = _allGivers.FirstOrDefault(s => s is T);
            
            if (giver == null)
                throw new ArgumentException("Incorrect type of giver");

            return giver;
        }
    }
    
    
}