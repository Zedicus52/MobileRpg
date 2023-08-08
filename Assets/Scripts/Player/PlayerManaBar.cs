using MobileRpg.Core;

namespace MobileRpg.Player
{
    public class PlayerManaBar : ValueBar
    {
        private PlayerEntity _playerEntity;
        private void Awake()
        {
            _playerEntity = GameBehaviour.Instance.PlayerBehaviour.PlayerEntity;
            Initialize(0.0f, _playerEntity.GetCurrentMaxMana());
        }

        private void OnEnable()
        {
            _playerEntity.ManaChanged += OnValueChanged;
            _playerEntity.MaxManaChanged += UpdateMaxValue;
        }

        private void OnDisable()
        {
            _playerEntity.ManaChanged -= OnValueChanged;
            _playerEntity.MaxManaChanged -= UpdateMaxValue;
        }
    }
}