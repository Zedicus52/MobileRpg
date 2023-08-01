using System;
using MobileRpg.Core;
using MobileRpg.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace MobileRpg.Monsters
{
    [RequireComponent(typeof(HealthBar))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Monster : MonoBehaviour, IEntity
    {
        public event Action<float> HealthChanged;
        public event Action<Monster> MonsterDie;
        public event Action<Monster> HasReachedDestinationPoint;

        public int GoldAmount => _currentGoldAmount;
        public IAttackConfig AttackConfig => _attackConfig;
        
        [SerializeField] private Animator _animator;

        private float _currentHealth;
        private float _currentDamage;
        private int _currentGoldAmount;

        private HealthBar _healthBar;
        private NavMeshAgent _navMeshAgent;
        private Transform _transform;
        private IAttackConfig _attackConfig;

        private bool _finishPath;
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");

        public void Initialize(float health, float damage, int goldAmount ,Vector3 destinationPoint, IAttackConfig config)
        {
            _currentHealth = health;
            _currentDamage = damage;
            _currentGoldAmount = goldAmount;
            _attackConfig = config;
            
            _healthBar.Initialize(0, _currentHealth);
            _navMeshAgent.SetDestination(destinationPoint);
            _finishPath = false;
            _animator.SetBool(IsMoving,true);
        }
        
        private void Awake()
        {
            _healthBar = GetComponent<HealthBar>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _transform = GetComponent<Transform>();
        }
        
        private void OnEnable()
        {
            HealthChanged += _healthBar.OnValueChanged;
        }

        private void FixedUpdate()
        {
            if(_finishPath)
                return;
            
            if (DestinationReached())
            {
                _finishPath = true;
                _animator.SetBool(IsMoving,false);
                HasReachedDestinationPoint?.Invoke(this);
            }
        }

        private void OnDisable()
        {
            HealthChanged -= _healthBar.OnValueChanged;
        }
        
        
        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            HealthChanged?.Invoke(_currentHealth);
            
            if(_currentHealth <= 0)
                MonsterDie?.Invoke(this);
        }
        
        private bool DestinationReached()
        { 
            if (_navMeshAgent.pathPending)
                return Vector3.Distance(_transform.position, _navMeshAgent.pathEndPosition) <= _navMeshAgent.stoppingDistance;
            
            return (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance);
        }
    }
}