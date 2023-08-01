using UnityEngine;

namespace MobileRpg.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Escape config", menuName = "Configs/Player Configs/Escape", order = 0)]
    public class PlayerEscapeConfig : ScriptableObject
    {
        [Range(0.0f, 0.9f)]
        [SerializeField] private float _chanceToEscape;

        public float GetChanceToEscape() => _chanceToEscape;
    }
}