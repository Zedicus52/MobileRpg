using System;
using UnityEngine;

namespace MobileRpg.Interfaces
{
    public interface IEntity
    {
        public event Action<float> HealthChanged;
        
        void TakeDamage(float damage);
    }
}