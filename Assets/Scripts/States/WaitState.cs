using MobileRpg.Interfaces;
using UnityEngine;

namespace MobileRpg.States
{
    public class WaitState : BaseState
    {
        public WaitState(IStateSwitcher switcher, IEntity entity) 
            : base(switcher, entity)
        {
        }
        
        public override void Attack()
        {
            Debug.Log("Attack from wait");
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