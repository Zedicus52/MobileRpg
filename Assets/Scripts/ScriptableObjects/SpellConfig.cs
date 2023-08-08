using MobileRpg.Enums;
using UnityEngine;

namespace MobileRpg.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Spell config", menuName = "Configs/Spells configs/Spell", order = 0)]
    public class SpellConfig : ScriptableObject
    {
        public int Id => _id;
        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
        public float ManaCost => _manaCost;
        public int ApplyStepsCount => _applyStepsCount;
        public SpellType SpellType => _spellType;

        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField, TextArea] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private float _manaCost;
        [SerializeField] private float _minDamage;
        [SerializeField] private float _maxDamage;
        [SerializeField] private int _applyStepsCount = 1;
        [SerializeField] private SpellType _spellType;

        public float GetDamage() => Random.Range(_minDamage, _maxDamage);
    }
}