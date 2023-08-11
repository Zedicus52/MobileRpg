using System;
using MobileRpg.Player;

namespace MobileRpg.Givers
{
    public abstract class Giver
    {
        public event Action EndedInteraction; 
        
        protected readonly PlayerBehaviour _playerBehaviour;

        protected Giver(PlayerBehaviour playerBehaviour)
        {
            _playerBehaviour = playerBehaviour;
        }

        public abstract void StartInteraction();

        public abstract void Subscribe();
        public abstract void UnSubscribe();
        
        protected void OnEndedInteraction() => EndedInteraction?.Invoke();
    }
}