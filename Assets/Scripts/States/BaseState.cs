using System;
using MobileRpg.Interfaces;

namespace MobileRpg.States
{
    public abstract class BaseState
    {
        protected readonly IStateSwitcher _switcher;
        protected IEntity _entity;

        protected BaseState(IStateSwitcher switcher, IEntity entity)
        {
            _switcher = switcher;
            _entity = entity;
        }

        public virtual void SetEntity(IEntity newEntity) => _entity = newEntity;

        public abstract void Attack();
        public abstract bool Escape();
        public abstract void MagicAttack();
        public abstract void Wait();        
        
    }
}