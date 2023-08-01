using System;
using System.Collections.Generic;
using System.Linq;
using MobileRpg.Interfaces;
using MobileRpg.Monsters;
using MobileRpg.ScriptableObjects;
using MobileRpg.States;
using UnityEngine;

namespace MobileRpg.Core
{
    public class PlayerBehaviour : MonoBehaviour, IStateSwitcher
    {
        public event Action<BaseState, bool> StateChanged;
        public PlayerEntity PlayerEntity => _playerEntity;
        
        [SerializeField] private MonstersBehaviour _monstersBehaviour;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private HealthBar _healthBar;
        
        private PlayerEntity _playerEntity;
        private IEntity _monster;
        private BaseState _currentState;
        private List<BaseState> _allStates;
        private bool _canInteractWithMonster;
        private bool _killedMonster;

        private void Awake()
        {
            _playerEntity = new PlayerEntity(_playerConfig);
            _healthBar.Initialize(0.0f,_playerConfig.GetHealth);
        }

        private void Start()
        {
            _canInteractWithMonster = false;
            _monster = _monstersBehaviour.CurrentMonster;
            _allStates = new List<BaseState>()
            {
                new AttackState(this, _monster, _playerConfig.GetAttackConfig()),
                new MagicAttackState(this, _monster),
                new EscapeState(this, _playerEntity, _monster),
                new WaitState(this, _playerEntity)
            };
            _currentState = _allStates[0];
        }

        private void OnEnable()
        {
            _monstersBehaviour.StateChangedToWait += OnMonsterStateChangedToWait;
            _playerEntity.HealthChanged += _healthBar.OnValueChanged;
            _playerEntity.MaxHealthChanged += _healthBar.UpdateMaxValue;
        }

        
        private void OnDisable()
        {
            _monstersBehaviour.StateChangedToWait -= OnMonsterStateChangedToWait;
            _playerEntity.HealthChanged -= _healthBar.OnValueChanged;
            _playerEntity.MaxHealthChanged -= _healthBar.UpdateMaxValue;
        }
        
        
        private void OnMonsterStateChangedToWait(BaseState monsterState)
        {
            if (monsterState is WaitState)
                SwitchState<AttackState>();
        }

        public void Attack()
        {
            if (_canInteractWithMonster == false)
                return;


            if(_currentState is not AttackState)
                SwitchState<AttackState>();
            
            _currentState.Attack();
        }

        public void MagicAttack()
        {
            if (_canInteractWithMonster == false)
                return;
            
            if(_currentState is not MagicAttackState)
                SwitchState<MagicAttackState>();
            
            _currentState.MagicAttack();
        } 

        public void Escape()
        {
            if (_canInteractWithMonster == false)
                return;
            
            if(_currentState is not EscapeState)
                SwitchState<EscapeState>();
            
            _currentState.Escape();
        }

        public void Wait() => _currentState.Wait();

        public void SwitchState<T>() where T : BaseState
        {
            BaseState state = _allStates.FirstOrDefault(s => s is T);
            _currentState = state;
            
            StateChanged?.Invoke(_currentState, _killedMonster);
        }

        public void OnMonsterReachedFightPosition(IEntity entity)
        {
             _canInteractWithMonster = true;
             _killedMonster = false;
             foreach (BaseState state in _allStates)
             {
                 if(state is not WaitState)
                     state.SetEntity(entity);
             }
        }

        public void OnMonsterDie(Monster entity)
        {
            _killedMonster = true;
            _canInteractWithMonster = false;
            _playerEntity.AddGold(entity.GoldAmount);
            SwitchState<AttackState>();
        }
        

    }
}