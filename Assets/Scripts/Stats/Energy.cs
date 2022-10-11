using System;
using Interfaces;
using UnityEngine;

namespace Stats
{
    public class Energy : IUiElement<float>
    {
        public float MaxEnergy {get; private set; }
        public float RestorePerTick { get; private set; }
        public event Action<float> OnValueChange;

        private float currentEnergy;
        
        public Energy(float maxEnergy, float energyRestoreSpeedInSeconds)
        {
            MaxEnergy = maxEnergy;
            currentEnergy = MaxEnergy;
            RestorePerTick = energyRestoreSpeedInSeconds;
        }

        public bool TryUseEnergy(float energyCost)
        {
            if (IsEnoughtEnergy(energyCost))
            {
                currentEnergy -= energyCost;
                OnValueChange?.Invoke(currentEnergy);
                return true;
            }

            return false;   
        }
        
        public bool IsEnoughtEnergy(float energyCost)
        {
            return currentEnergy >= energyCost;
        }

        public bool IsMaxEnergy()
        {
            return  MaxEnergy == currentEnergy;
        }
        
        public void RestoreEnergy(float value)
        {
            currentEnergy += value;
            ValidateEnergy();
        }
        
        private void ValidateEnergy()
        {
            currentEnergy = Mathf.Min(currentEnergy, MaxEnergy);
            OnValueChange?.Invoke(currentEnergy);
        }
    }
}