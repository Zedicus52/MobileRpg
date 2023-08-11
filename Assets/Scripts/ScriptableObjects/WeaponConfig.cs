using UnityEngine;

namespace MobileRpg.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Weapon Config", menuName = "Configs/Weapons Configs/Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        public int Id => _id;
        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
        public float DamageMultiplier => _damageMultiplayer;

        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField, TextArea] private string _description;
        [SerializeField] private Sprite _icon;
        [Range(0.1f, 0.9f), SerializeField] private float _damageMultiplayer = 0.1f;
    }
}