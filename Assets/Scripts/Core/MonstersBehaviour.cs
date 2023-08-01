using System;
using System.Collections.Generic;
using System.Linq;
using MobileRpg.Interfaces;
using MobileRpg.Monsters;
using MobileRpg.States;
using UnityEngine;

namespace MobileRpg.Core
{
    public class MonstersBehaviour : MonoBehaviour , IStateSwitcher
    {
        public event Action SuccesfullEscapeFromMonster; 
        public event Action FailtureEscapeFromMonster;
        

        public event Action<BaseState> StateChangedToWait; 
        public IEntity CurrentMonster => _currentMonster;

        [SerializeField] private PlayerBehaviour _playerBehaviour;
        [SerializeField] private WavesHandler _wavesHandler;

        private Monster _currentMonster;
        private IEntity _playerEntity;
        private BaseState _currentState;
        private List<BaseState> _allStates;
        

        private void Start()
        {
            if (_currentMonster == null)
                CreateNewMonster();
            
            _playerEntity = _playerBehaviour.PlayerEntity;
            _currentMonster.HasReachedDestinationPoint += _playerBehaviour.OnMonsterReachedFightPosition;
            _currentMonster.MonsterDie += _playerBehaviour.OnMonsterDie;
            _allStates = new List<BaseState>()
            {
                new WaitState(this, _currentMonster),
                new AttackState(this, _playerEntity, _currentMonster.AttackConfig)
            };
            _currentState = _allStates[0];
        }

        private void OnEnable()
        {
            _playerBehaviour.StateChanged += OnPlayerStateChanged;
        }

        private void OnDisable()
        {
            _playerBehaviour.StateChanged -= OnPlayerStateChanged;
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
            obj.HasReachedDestinationPoint -= _playerBehaviour.OnMonsterReachedFightPosition;
            obj.MonsterDie -= _playerBehaviour.OnMonsterDie;
            Destroy(obj.gameObject);
            CreateNewMonster();
        }

        private void OnEscapeFromMonster()
        {
            SwitchState<WaitState>();
            _currentMonster.MonsterDie -= OnMonsterDie;
            _currentMonster.HasReachedDestinationPoint -= _playerBehaviour.OnMonsterReachedFightPosition;
            _currentMonster.MonsterDie -= _playerBehaviour.OnMonsterDie;
            Destroy(_currentMonster.gameObject);
            CreateNewMonster();
        }

        private void CreateNewMonster()
        {
            _currentMonster = _wavesHandler.GetNextMonster();
            
            if(_currentMonster == null)
                return;
            
            _currentMonster.MonsterDie += OnMonsterDie;
            _currentMonster.MonsterDie += _playerBehaviour.OnMonsterDie;
            if (_playerBehaviour != null)
                _currentMonster.HasReachedDestinationPoint += _playerBehaviour.OnMonsterReachedFightPosition;
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