using UnityEngine;

namespace MobileRpg.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Block config", menuName = "Configs/Player Configs/Block", order = 0)]
    public class PlayerBlockConfig : ScriptableObject
    {
        [Range(0.0f,0.9f)]
        [SerializeField] private float _chanceToBlock;

        public float GetBlockChance() => _chanceToBlock;
    }
}