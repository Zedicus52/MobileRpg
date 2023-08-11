using MobileRpg.Interfaces;
using UnityEngine;

namespace MobileRpg.States
{
    public class AttackState : BaseState
    {
        private readonly IAttackConfig _attackConfig;
        
        public AttackState(IStateSwitcher switcher, IEntity entity, IAttackConfig attackConfig) 
            : base(switcher, entity)
        {
            _attackConfig = attackConfig;
        }
        
        public override void Attack()
        {
            Debug.Log(_attackConfig.GetDamage());
            _entity.TakeDamage(_attackConfig.GetDamage());
            _switcher.SwitchState<WaitState>();
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