using UnityEngine;

namespace MobileRpg.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Player magic config", menuName = "Configs/Player Configs/Magic", order = 0)]
    public class PlayerMagicConfig : ScriptableObject
    {
        public float Mana => _mana;
        public SpellConfig GetCurrentSpell() => _currentSpell;
        
        [SerializeField] private float _mana;
        [SerializeField] private SpellConfig _currentSpell;
    }
}