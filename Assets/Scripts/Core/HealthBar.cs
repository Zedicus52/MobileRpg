using System;
using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.Core
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        public void Initialize(float minValue, float maxValue)
        {
            _slider.minValue = minValue;
            _slider.maxValue = maxValue;
            _slider.value = maxValue;
        }
        
        public void Initialize(int minValue, int maxValue)
        {
            _slider.wholeNumbers = true;
            _slider.minValue = minValue;
            _slider.maxValue = maxValue;
            _slider.value = maxValue;
        }
        
        public void OnValueChanged(float value)
        {
            _slider.value = value;
        }

        public void UpdateMaxValue(float newMaxValue)
        {
            _slider.maxValue = newMaxValue;
        }
        
        public void OnValueChanged(int value)
        {
            _slider.value = value;
        }
    }
}