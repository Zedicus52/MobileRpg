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
        public static GameBehaviour Instance { get; private set; }
        
        public PlayerBehaviour PlayerBehaviour { get; private set; }
        public MonstersBehaviour MonstersBehaviour { get; private set; }
        
        public IWavesHandler WavesHandler { get; private set; }
        
        [Header("Waves handler configuration")]
        [SerializeField] private WavesContainer _wavesContainer;
        [SerializeField] private ConfiguredMonsterFactory _monsterFactory;

        [Space] [Header("Player Behaviour configuration")] 
        [SerializeField] private PlayerConfig _playerConfig;
        

        private void Awake()
        {
            Initialize();
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }
            
            Destroy(gameObject);
        }

        private void Initialize()
        {
            WavesHandler = new WavesHandler(_wavesContainer, _monsterFactory);
            
            PlayerBehaviour = new PlayerBehaviour(_playerConfig);
            MonstersBehaviour = new MonstersBehaviour(PlayerBehaviour, WavesHandler);
            MonstersBehaviour.Subscribe();
            
            PlayerBehaviour.InitializeMonsterBehaviour(MonstersBehaviour);
            PlayerBehaviour.Subscribe();
        }

        private void Start()
        {
            WavesHandler.StartWavesSpawning();
        }

        private void OnEnable()
        {
            PlayerBehaviour.PlayerEntity.PlayerDie += OnPlayerDie;
        }

        private void OnDisable()
        {
            PlayerBehaviour.PlayerEntity.PlayerDie -= OnPlayerDie;
        }

        private void OnPlayerDie() => ReloadCurrentScene();
        

        private void OnDestroy()
        {
            PlayerBehaviour.UnSubscribe();
            MonstersBehaviour.UnSubscribe();
        }


        private void ReloadCurrentScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}