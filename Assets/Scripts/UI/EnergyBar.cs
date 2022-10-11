using Stats;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class EnergyBar : MonoBehaviour
    {
        private Energy energy;
        private Slider slider;
        
        [Inject]
        private void Construct(Energy energy)
        {
            this.energy = energy;
            slider = GetComponent<Slider>();
        }

        private void OnEnable()
        {
            energy.OnValueChange += SetValue;
        }

        private void OnDisable()
        {
            energy.OnValueChange -= SetValue;
        }

        private void SetValue(float value)
        {
            var sliderValue = value / energy.MaxEnergy;
            slider.value = sliderValue;
        }
    }
}