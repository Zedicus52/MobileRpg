using System;
using System.Collections;
using MobileRpg.Core;
using TMPro;
using UnityEngine;

namespace MobileRpg.UI
{
    public class ActionsResultHandler : MonoBehaviour
    {
        [SerializeField] private MonstersBehaviour _monstersBehaviour;
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private float _timeForTextDisplaying;

        private void OnEnable()
        {
            _monstersBehaviour.SuccesfullEscapeFromMonster += OnEscapeFromMonster;
            _monstersBehaviour.FailtureEscapeFromMonster += OnFailtureEscapeFromMonster;
        }

        private void OnDisable()
        {
            _monstersBehaviour.SuccesfullEscapeFromMonster -= OnEscapeFromMonster;
            _monstersBehaviour.FailtureEscapeFromMonster -= OnFailtureEscapeFromMonster;

        }
        private void OnEscapeFromMonster() => UpdateText("You escape from monster!");
        private void OnFailtureEscapeFromMonster() => UpdateText("You couldn't escape from monster!");


        private void UpdateText(string newMessage)
        {
            _messageText.text = newMessage;
            StartCoroutine(StartTextDisplaying());
        }

        private IEnumerator StartTextDisplaying()
        {
            yield return new WaitForSeconds(_timeForTextDisplaying);
            _messageText.text = "";
        }
    }
}