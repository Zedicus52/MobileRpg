using System;
using System.Collections.Generic;
using MobileRpg.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.UI
{
    public class SpellGiverDisplay : MonoBehaviour
    {
        public event Action EndedInteraction;
        public event Action<SpellConfig> GotSpell;
        
        [SerializeField] private Transform _rootPanel;
        [SerializeField] private GridLayoutGroup _spellsContainer;
        [SerializeField] private SpellDisplay _prefab;
        [SerializeField] private Button _closeButton;

        private List<SpellDisplay> _spells = new();

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnClose);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnClose);
        }
        
        public void ShowSpells(List<SpellConfig> spells)
        {
            foreach (SpellConfig config in spells)
            {
                SpellDisplay display = Instantiate(_prefab, Vector3.zero, Quaternion.identity, _spellsContainer.transform);
                display.Initialize(config);
                display.Click += OnSpellClick;
                _spells.Add(display);
            }
            _rootPanel.gameObject.SetActive(true);
        }
        
        private void OnClose()
        {
            _rootPanel.gameObject.SetActive(false);
            foreach (var spell in _spells)
            {
                spell.Click -= OnSpellClick;
                Destroy(spell.gameObject);
            }
            EndedInteraction?.Invoke();
            _spells.Clear();
        }

        private void OnSpellClick(SpellConfig config)
        {
            GotSpell?.Invoke(config);
            OnClose();
        }
        
    }
}