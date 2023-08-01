using MobileRpg.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Bonus config", menuName = "Configs/Bonuses/Bonus", order = 0)]
    public class BonusConfig : ScriptableObject
    {
        public Sprite BonusIcon => _bonusIcon;
        public string BonusName => _bonusName;
        public string BonusDescription => _bonusDescription;
        public BonusType BonusType => _bonusType;
        public BonusEffectType EffectType => _effectType;
        
        
        [SerializeField] private Sprite _bonusIcon;
        [SerializeField] private string _bonusName;
        [TextArea, SerializeField] private string _bonusDescription;
        [SerializeField] private BonusType _bonusType;
        [SerializeField] private BonusEffectType _effectType;
        [Range(0.1f, 0.9f)]
        [SerializeField] private float _percent;
        
        public float GetBonusAmount(float currentValue) => _percent * currentValue;
    }
}