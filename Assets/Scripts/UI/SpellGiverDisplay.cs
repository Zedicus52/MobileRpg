using System.Collections.Generic;
using MobileRpg.Core;
using MobileRpg.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MobileRpg.UI
{
    public class SpellGiverDisplay : MonoBehaviour
    {
        [SerializeField] private SpellGiver _spellsGiver;
        [SerializeField] private Transform _rootPanel;
        [SerializeField] private GridLayoutGroup _spellsContainer;
        [SerializeField] private SpellDisplay _prefab;
        [SerializeField] private Button _closeButton;

        private List<SpellDisplay> _spells = new();

        private void OnEnable()
        {
            _spellsGiver.ShowSpells += OnShowSpell;
            _closeButton.onClick.AddListener(OnClose);
        }

        private void OnDisable()
        {
            _spellsGiver.ShowSpells -= OnShowSpell;
            _closeButton.onClick.RemoveListener(OnClose);
        }
        
        private void OnShowSpell(List<SpellConfig> spells)
        {
            float containerHeight = ((spells.Count / 2f) * _spellsContainer.cellSize.y) +
                                    ((spells.Count / 2f) * _spellsContainer.spacing.y);

            RectTransform r = ((RectTransform)_spellsContainer.transform);
            r.sizeDelta = new Vector2(r.sizeDelta.x, containerHeight);

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
            _spellsGiver.EndInteraction();
        }

        private void OnSpellClick(SpellConfig config)
        {
            _spellsGiver.GiveSpell(config);
            OnClose();
        }
        
    }
}