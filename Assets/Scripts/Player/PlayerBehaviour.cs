using System;
using System.Collections.Generic;
using System.Linq;
using MobileRpg.Core;
using MobileRpg.Interfaces;
using MobileRpg.Models;
using MobileRpg.Monsters;
using MobileRpg.ScriptableObjects;
using MobileRpg.States;
using MobileRpg.UI;
using UnityEngine;

namespace MobileRpg.Player
{
    public class PlayerBehaviour : IStateSwitcher
    {
        public event Action<BaseState, bool> StateChanged;
        public PlayerEntity PlayerEntity { get; private set; }
        public Timer Timer => _timer;
        
        
        private readonly PlayerConfig _playerConfig;
        private readonly Timer _timer;

        private Monster _monster;
        private MonstersBehaviour _monstersBehaviour;
        private BaseState _currentState;
        private List<BaseState> _allStates;
        private bool _canInteractWithMonster;
        private bool _killedMonster;
        private AttackModel _attackModel;
        


        public PlayerBehaviour(PlayerConfig config, Timer timer)
        {
            _timer = timer;
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
            _timer.TimerFinished += OnTimerFinished;
        }
        

        public void UnSubscribe()
        {
            _monstersBehaviour.StateChangedToWait -= OnMonsterStateChangedToWait;
            _monstersBehaviour.EscapeFromMonster -= EscapeFromMonster;
            _monstersBehaviour.MonsterSpawned -= OnNewMonsterSpawned;
            _timer.TimerFinished -= OnTimerFinished;
            PlayerEntity.WeaponChanged -= _attackModel.UpdateWeapon;
        }
        
        private void OnTimerFinished() => SwitchState<WaitState>();

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
            _canInteractWithMonster = false;
            _killedMonster = true;
            _timer.StopTimer();
        }
        
        
        private void OnMonsterStateChangedToWait(BaseState monsterState)
        {
            if (monsterState is WaitState)
            {
                SwitchState<AttackState>();
                if(_killedMonster == false)
                    _timer.StartTimer();
            }
                
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
             _timer.StartTimer();
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
            _timer.StopTimer();
        }

        private void InitializeStates()
        {
            PlayerAttackConfig config = _playerConfig.GetAttackConfig();
            _attackModel = new AttackModel(config.GetBaseAttack(), config.GetAttackMultiplier(),
                config.GetWeapon().DamageMultiplier);
            PlayerEntity.WeaponChanged += _attackModel.UpdateWeapon;
            
            _allStates = new List<BaseState>()
            {
                new AttackState(this, _monster, _attackModel),
                new MagicAttackState(this, _monster, PlayerEntity, _monstersBehaviour),
                new EscapeState(this, PlayerEntity, _monster),
                new WaitState(this, PlayerEntity)
            };
            _currentState = _allStates[0];
        }
    }
}