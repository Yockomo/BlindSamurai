using System;
using System.Collections;
using Interfaces;
using UnityEngine;

namespace Stats
{
    public class Energy : IUiElement<float>
    {
        private float maxEnergy;
        private float currentEnergy;
        private float energyRestoreSpeedInSeconds;

        private float restorePerTick;
        
        public event Action<float> OnValueChange;
        
        public bool Active {get; private set;}

        public Energy(float maxEnergy, float energyRestoreSpeedInSeconds)
        {
            this.maxEnergy = maxEnergy;
            currentEnergy = maxEnergy;
            restorePerTick = energyRestoreSpeedInSeconds / 10;
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

        public IEnumerator ChangeActiveState()
        {
            Active = !Active;
            
            if (Active)
            {
                while (!Active && currentEnergy < maxEnergy)
                {
                    currentEnergy += restorePerTick;
                    OnValueChange?.Invoke(currentEnergy);
                    yield return new WaitForSeconds(restorePerTick);
                }

                currentEnergy = Mathf.Min(currentEnergy, maxEnergy);
            }
        }
    }
}