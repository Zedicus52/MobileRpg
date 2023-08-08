using MobileRpg.Enums;
using MobileRpg.ScriptableObjects;

namespace MobileRpg.Models
{
    public class SpellModel
    {
        public int Id => Config.Id;
        public SpellType SpellType => Config.SpellType;
        public SpellConfig Config { get; private set; }
        public int RemainStepsCount { get; private set; }

        public SpellModel(SpellConfig config)
        {
            Config = config;
            RemainStepsCount = config.ApplyStepsCount;
        }

        public void ApplySpell() => RemainStepsCount -= 1;

        public bool CanApplySpell() => RemainStepsCount > 0;
    }
}