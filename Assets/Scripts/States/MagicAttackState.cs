using MobileRpg.Interfaces;

namespace MobileRpg.States
{
    public class MagicAttackState : BaseState
    {
        public MagicAttackState(IStateSwitcher switcher, IEntity entity) : base(switcher, entity)
        {
        }

        public override void Attack()
        {
        }

        public override bool Escape()
        {
            return false;
        }

        public override void MagicAttack()
        {
            
        }

        public override void Wait()
        {
        }
    }
}