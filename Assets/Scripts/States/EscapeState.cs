using MobileRpg.Core;
using MobileRpg.Interfaces;
using MobileRpg.Monsters;
using MobileRpg.Player;
using UnityEngine;

namespace MobileRpg.States
{
    public class EscapeState : BaseState
    {
        private IEntity _monsterEntity;

        public EscapeState(IStateSwitcher switcher, IEntity player, IEntity monster) 
            : base(switcher,  player)
        {
            _monsterEntity = monster;
        }
        
        public override void SetEntity(IEntity newEntity)
        {
            if (newEntity is Monster)
                _monsterEntity = newEntity;
            else
                base.SetEntity(newEntity);
        }
        
        public override void Attack()
        {
            
        }

        public override bool Escape()
        {
            if(Random.value < (_entity as PlayerEntity).GetCurrentEscapeChance())
                return true;
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