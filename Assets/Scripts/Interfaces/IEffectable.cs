using MobileRpg.Models;
using MobileRpg.ScriptableObjects;

namespace MobileRpg.Interfaces
{
    public interface IEffectable
    {
        void AddSpell(SpellModel config);

        void RemoveSpell(SpellModel config);

        void RemoveAllSpells();
    }
}