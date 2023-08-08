using MobileRpg.Core;

namespace MobileRpg.Player
{
    public class PlayerHealthBar : ValueBar
    {
        private PlayerEntity _playerEntity;
        private void Awake()
        {
            _playerEntity = GameBehaviour.Instance.PlayerBehaviour.PlayerEntity;
            Initialize(0.0f, _playerEntity.GetCurrentMaxHealth());
        }

        private void OnEnable()
        {
            _playerEntity.HealthChanged += OnValueChanged;
            _playerEntity.MaxHealthChanged += UpdateMaxValue;
        }

        private void OnDisable()
        {
            _playerEntity.HealthChanged -= OnValueChanged;
            _playerEntity.MaxHealthChanged -= UpdateMaxValue;
        }
    }
}