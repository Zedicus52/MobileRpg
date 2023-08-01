using System;
using MobileRpg.Factories.MonsterFactory;
using MobileRpg.Interfaces;
using MobileRpg.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MobileRpg.Core
{
    public class GameBehaviour : MonoBehaviour
    {
        public static GameBehaviour Instance;
        
        [Header("Waves handler configuration")]
        [SerializeField] private WavesContainer _wavesContainer;
        [SerializeField] private ConfiguredMonsterFactory _monsterFactory;
        
        [SerializeField]private PlayerBehaviour _playerBehaviour;
        [SerializeField]private MonstersBehaviour _monstersBehaviour;
        [SerializeField]private WavesHandler _wavesHandler;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                return;
            }
            
            Destroy(gameObject);
        }

        private void OnEnable()
        {
            _playerBehaviour.PlayerEntity.PlayerDie += OnPlayerDie;
            //_wavesHandler.WavesAreOver += OnWavesAreOver;
        }

        private void OnDisable()
        {
            _playerBehaviour.PlayerEntity.PlayerDie -= OnPlayerDie;
            //_wavesHandler.WavesAreOver -= OnWavesAreOver;
        }

        private void OnPlayerDie() => ReloadCurrentScene();

        private void OnWavesAreOver() => ReloadCurrentScene();
        

        
        private void ReloadCurrentScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}