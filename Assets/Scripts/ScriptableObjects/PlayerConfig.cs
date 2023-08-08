using UnityEngine;

namespace MobileRpg.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Player Config", menuName = "Configs/Player Configs/Player", order = 0)]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private PlayerAttackConfig _attackConfig;
        [SerializeField] private PlayerEscapeConfig _escapeConfig;
        [SerializeField] private PlayerMagicConfig _magicConfig;
        [SerializeField] private int _goldAmount;
        [SerializeField] private float _health;
        

        public PlayerAttackConfig GetAttackConfig() => _attackConfig;
        public PlayerEscapeConfig GetEscapeConfig() => _escapeConfig;
        public PlayerMagicConfig GetMagicConfig() => _magicConfig;
        public float Health => _health;
        public float Mana => _magicConfig.Mana;
    }
}