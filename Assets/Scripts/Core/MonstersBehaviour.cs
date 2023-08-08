using System;
using System.Collections.Generic;
using System.Linq;
using MobileRpg.Enums;
using MobileRpg.Interfaces;
using MobileRpg.Models;
using MobileRpg.Monsters;
using MobileRpg.Player;
using MobileRpg.ScriptableObjects;
using MobileRpg.States;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MobileRpg.Core
{
    public class MonstersBehaviour : IStateSwitcher, IEffectable
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
        private readonly List<SpellModel> _appliedSpells;


        public MonstersBehaviour(PlayerBehaviour playerBehaviour, IWavesHandler handler)
        {
            _playerEntity = playerBehaviour.PlayerEntity;
            _playerBehaviour = playerBehaviour;
            _wavesHandler = handler;
            _appliedSpells = new List<SpellModel>();
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

        private void Start()
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
            
            if(playerState is not AttackState)
            {
                ApplySpells();
            }
            
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
            
            RemoveAllSpells();

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

        private void ApplySpells()
        {
            foreach (var spell in _appliedSpells)
            {
                Debug.Log($"Apply spell with name: {spell.Config.Name}");
                if (spell.CanApplySpell())
                {
                    if(spell.SpellType == SpellType.DisposableActive || spell.SpellType == SpellType.ReusableActive)
                        _currentMonster.TakeDamage(spell.Config.GetDamage());
                    
                    spell.ApplySpell();
                }
            }

            _appliedSpells.RemoveAll(s => s.CanApplySpell() == false);
        }

        public void AddSpell(SpellModel config)
        {
            var first = _appliedSpells.FirstOrDefault(s => s.Id == config.Id);
            if(first != null)
                return;

            if (config.SpellType == SpellType.DisposableActive)
            {
                ApplyDisposableSpell(config);
                return;
            }
            
            _appliedSpells.Add(config);
        }

        private void ApplyDisposableSpell(SpellModel config)
        {
            if(config.SpellType == SpellType.DisposableActive)
                _currentMonster.TakeDamage(config.Config.GetDamage());
        }

        public void RemoveSpell(SpellModel config)
        {
            var first = _appliedSpells.FirstOrDefault(s => s.Id == config.Id);
            if(first == null)
                return;

            _appliedSpells.Remove(first);
        }

        public void RemoveAllSpells()
        {
            _appliedSpells.Clear();
        }
    }
}