using MobileRpg.Interfaces;
using MobileRpg.Models;
using MobileRpg.Player;
using MobileRpg.ScriptableObjects;
using UnityEngine;

namespace MobileRpg.States
{
    public class MagicAttackState : BaseState
    {
        private readonly IEffectable _effectable;
        private readonly PlayerEntity _player;
        public MagicAttackState(IStateSwitcher switcher, IEntity entity, 
            PlayerEntity player, IEffectable effectable) : base(switcher, entity)
        {
            _player = player;
            _effectable = effectable;
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
            if (_player.CanUseCurrentSpell())
            {
                _player.UseCurrentSpell();
                _effectable.AddSpell(new SpellModel(_player.GetCurrentSpell()));
                _switcher.SwitchState<WaitState>();
            }
            
        }

        public override void Wait()
        {
        }
        
    }
}