using UnityEngine;

namespace MobileRpg.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Player Config", menuName = "Configs/Player Configs/Player", order = 0)]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private PlayerAttackConfig _attackConfig;
        [SerializeField] private PlayerBlockConfig _blockConfig;
        [SerializeField] private PlayerEscapeConfig _escapeConfig;
        [SerializeField] private int _goldAmount;
        [SerializeField] private float _health;
        [SerializeField] private float _mana;

        public PlayerAttackConfig GetAttackConfig() => _attackConfig;
        public PlayerBlockConfig GetBlockConfig() => _blockConfig;
        public PlayerEscapeConfig GetEscapeConfig() => _escapeConfig;
        public float GetHealth => _health;
        public float GetMana => _mana;
    }
}