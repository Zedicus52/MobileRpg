using System;
using MobileRpg.Interfaces;
using MobileRpg.ScriptableObjects;

namespace MobileRpg.Player
{
    public class PlayerEntity : IEntity
    {
        public event Action PlayerDie;
        public event Action<int> GoldAmountChanged;
        
        public event Action<float> HealthChanged;
        public event Action<float> MaxHealthChanged; 
        
        public event Action<float> ManaChanged;
        public event Action<float> MaxManaChanged;

        public event Action<SpellConfig> SpellChanged;

        public event Action<WeaponConfig> WeaponChanged;

        public float GetCurrentHealth() => _currentHealth;
        public float GetCurrentMaxHealth() => _maxHealth;
        public float GetCurrentMaxMana() => _maxMana;
        public float GetCurrentMana() => _currentMana;
        public float GetCurrentEscapeChance() => _escapeChance;
        public SpellConfig GetCurrentSpell() => _currentSpell;
        public WeaponConfig GetCurrentWeapon() => _currentWeapon;
        
        private readonly PlayerConfig _playerConfig;
        
        private float _maxHealth;
        private float _currentHealth;
        
        private float _maxMana;
        private float _currentMana;

        private float _escapeChance;
        
        private int _currentGoldAmount;

        private SpellConfig _currentSpell;
        private WeaponConfig _currentWeapon;
        public PlayerEntity(PlayerConfig playerConfig)
        {
            _playerConfig = playerConfig;
            _currentHealth = _playerConfig.Health;
            _maxHealth = _playerConfig.Health;
            _maxMana = _playerConfig.Mana;
            _currentMana = _playerConfig.Mana;
            _escapeChance = playerConfig.GetEscapeConfig().GetChanceToEscape();
            _currentGoldAmount = 0;
            _currentSpell = playerConfig.GetMagicConfig().GetCurrentSpell();
            _currentWeapon = playerConfig.GetAttackConfig().GetWeapon();
        }

        public void AddGold(int gold)
        {
            _currentGoldAmount += gold;
            GoldAmountChanged?.Invoke(_currentGoldAmount);
        }
        
        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            
            HealthChanged?.Invoke(_currentHealth);
            
            if(_currentHealth <= 0)
                PlayerDie?.Invoke();
        }

        public void RestoreHealth(float health)
        {
            if (_currentHealth + health > _maxHealth)
                _currentHealth = _maxHealth;
            else
                _currentHealth += health;
            
            HealthChanged?.Invoke(_currentHealth);
        }

        public void RestoreMana(float mana)
        {
            if (_currentMana + mana > _maxMana)
                _currentMana = _maxMana;
            else
                _currentMana += mana;
            
            ManaChanged?.Invoke(_currentMana);
        }

        public void IncreaseMaxHealth(float amount)
        {
            _maxHealth += amount;
            MaxHealthChanged?.Invoke(_maxHealth);
        }

        public void IncreaseMaxMana(float amount)
        {
            _maxMana += amount;
            MaxManaChanged?.Invoke(_maxMana);
        }

        public void IncreaseEscapeChance(float amount)
        {
            _escapeChance += amount;
        }

        public bool CanUseCurrentSpell() => _currentMana >= _currentSpell.ManaCost;

        public void UseCurrentSpell()
        {
            if (CanUseCurrentSpell())
            {
                _currentMana -= _currentSpell.ManaCost;
                ManaChanged?.Invoke(_currentMana);
            }
        }

        public void UpdateSpell(SpellConfig config)
        {
            _currentSpell = config;
            SpellChanged?.Invoke(config);
        }
        
        public void UpdateWeapon(WeaponConfig config)
        {
            _currentWeapon = config;
            WeaponChanged?.Invoke(config);
        }
        
    }
}