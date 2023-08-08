using System;
using System.Collections.Generic;
using System.Linq;
using MobileRpg.Core;
using MobileRpg.Interfaces;
using MobileRpg.Monsters;
using MobileRpg.ScriptableObjects;
using MobileRpg.States;
using MobileRpg.UI;

namespace MobileRpg.Player
{
    public class PlayerBehaviour : IStateSwitcher
    {
        public event Action<BaseState, bool> StateChanged;
        public PlayerEntity PlayerEntity { get; private set; }
        
        
        private readonly PlayerConfig _playerConfig;

        private Monster _monster;
        private MonstersBehaviour _monstersBehaviour;
        private BaseState _currentState;
        private List<BaseState> _allStates;
        private bool _canInteractWithMonster;
        private bool _killedMonster;


        public PlayerBehaviour(PlayerConfig config)
        {
            _playerConfig = config;
            _canInteractWithMonster = false;
            PlayerEntity = new PlayerEntity(_playerConfig);
        }
        
        public void InitializeMonsterBehaviour(MonstersBehaviour behaviour)
        {
            _monstersBehaviour = behaviour;
        }

        public void Subscribe()
        {
            _monstersBehaviour.StateChangedToWait += OnMonsterStateChangedToWait;
            _monstersBehaviour.EscapeFromMonster += EscapeFromMonster;
            _monstersBehaviour.MonsterSpawned += OnNewMonsterSpawned;
        }
        

        public void UnSubscribe()
        {
            _monstersBehaviour.StateChangedToWait -= OnMonsterStateChangedToWait;
            _monstersBehaviour.EscapeFromMonster -= EscapeFromMonster;
            _monstersBehaviour.MonsterSpawned -= OnNewMonsterSpawned;
        }
        
        private void OnNewMonsterSpawned(Monster newMonster)
        {
            if(newMonster == null)
                return;

            if (_monster != null)
            {
                _monster.HasReachedDestinationPoint -= OnMonsterReachedFightPosition;
                _monster.MonsterDie -= OnMonsterDie;
            }

            _monster = newMonster;
            _monster.HasReachedDestinationPoint += OnMonsterReachedFightPosition;
            _monster.MonsterDie += OnMonsterDie;
            
            if(_allStates == null)
                InitializeStates();
        }
        
        private void EscapeFromMonster(Monster monster)
        {
            if (_monster == null) 
                return;
            
            _monster.HasReachedDestinationPoint -= OnMonsterReachedFightPosition;
            _monster.MonsterDie -= OnMonsterDie;
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

        private void OnMonsterReachedFightPosition(IEntity entity)
        {
             _canInteractWithMonster = true;
             _killedMonster = false;
             foreach (BaseState state in _allStates)
             {
                 if(state is not WaitState)
                     state.SetEntity(entity);
             }
        }

        private void OnMonsterDie(Monster entity)
        {
            _killedMonster = true;
            _canInteractWithMonster = false;
            PlayerEntity.AddGold(entity.GoldAmount);
            SwitchState<AttackState>();
        }

        private void InitializeStates()
        {
            _allStates = new List<BaseState>()
            {
                new AttackState(this, _monster, _playerConfig.GetAttackConfig()),
                new MagicAttackState(this, _monster, PlayerEntity, _monstersBehaviour),
                new EscapeState(this, PlayerEntity, _monster),
                new WaitState(this, PlayerEntity)
            };
            _currentState = _allStates[0];
        }
    }
}