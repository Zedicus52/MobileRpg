using System;
using System.Collections.Generic;
using System.Linq;
using MobileRpg.Interfaces;
using MobileRpg.Monsters;
using MobileRpg.States;
using Object = UnityEngine.Object;

namespace MobileRpg.Core
{
    public class MonstersBehaviour : IStateSwitcher
    {
        public event Action SuccesfullEscapeFromMonster; 
        public event Action FailtureEscapeFromMonster;

        public event Action<BaseState> StateChangedToWait;
        public event Action<Monster> MonsterSpawned;
        public event Action<Monster> EscapeFromMonster;
        public IEntity CurrentMonster => _currentMonster;
        
        private readonly IWavesHandler _wavesHandler;

        private Monster _currentMonster;
        private readonly IEntity _playerEntity;
        private readonly PlayerBehaviour _playerBehaviour;
        private BaseState _currentState;
        private List<BaseState> _allStates;


        public MonstersBehaviour(PlayerBehaviour playerBehaviour, IWavesHandler handler)
        {
            _playerEntity = playerBehaviour.PlayerEntity;
            _playerBehaviour = playerBehaviour;
            _wavesHandler = handler;
        }


        public void Subscribe()
        {
            _playerBehaviour.StateChanged += OnPlayerStateChanged;
            _wavesHandler.WavesSpawningStarted += Start;
            _wavesHandler.WavesSpawningResume += Start;
        }

        public void UnSubscribe()
        {
            _playerBehaviour.StateChanged -= OnPlayerStateChanged;
            _wavesHandler.WavesSpawningStarted -= Start;
            _wavesHandler.WavesSpawningResume -= Start;
        }

        public void Start()
        {
            if (_currentMonster == null)
                CreateNewMonster();
            
            _allStates = new List<BaseState>()
            {
                new WaitState(this, _currentMonster),
                new AttackState(this, _playerEntity, _currentMonster.AttackConfig)
            };
            _currentState = _allStates[0];
        }

        private void OnPlayerStateChanged(BaseState playerState, bool killedMonster)
        {
            if(killedMonster)
                return;
            
            SwitchState<AttackState>();

            if (playerState is EscapeState)
            {
                if (playerState.Escape() == false)
                {
                    _currentState.Attack();
                    FailtureEscapeFromMonster?.Invoke();
                }
                else
                {
                    SuccesfullEscapeFromMonster?.Invoke();
                    OnEscapeFromMonster();
                    SwitchState<WaitState>();
                }
            }
            else if (playerState is WaitState)
            {
                _currentState.Attack();
            }
        }

        private void OnMonsterDie(Monster obj)
        {
            if (_currentMonster != obj) 
                return;
            
            SwitchState<WaitState>();

            obj.MonsterDie -= OnMonsterDie;
            Object.Destroy(obj.gameObject);
            CreateNewMonster();
        }

        private void OnEscapeFromMonster()
        {
            SwitchState<WaitState>();
            _currentMonster.MonsterDie -= OnMonsterDie;
            EscapeFromMonster?.Invoke(_currentMonster);
            Object.Destroy(_currentMonster.gameObject);
            CreateNewMonster();
        }

        private void CreateNewMonster()
        {
            _currentMonster = _wavesHandler.GetNextMonster();
            
            if(_currentMonster == null)
                return;
            
            _currentMonster.MonsterDie += OnMonsterDie;
            MonsterSpawned?.Invoke(_currentMonster);

            _currentMonster.MonsterDie += OnMonsterDie;
        }

        public void SwitchState<T>() where T : BaseState
        {
            BaseState state = _allStates.FirstOrDefault(s => s is T);
            _currentState = state;
            
            if(_currentState is WaitState)
                StateChangedToWait?.Invoke(_currentState);
        }
        
    }
}