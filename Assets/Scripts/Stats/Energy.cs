using System.Collections;
using UnityEngine;

namespace Stats
{
    public class Energy: MonoBehaviour
    {
        [SerializeField] private float maxEnergy;
        [SerializeField] private float currentEnergy;
        [SerializeField] private float energyRestoreSpeedInSeconds;

        private float restorePerTick;
        
        public bool Active {get; private set;}

        private void Awake()
        {
            currentEnergy = maxEnergy;
            restorePerTick = energyRestoreSpeedInSeconds / 10;
        }

        public bool TryUseEnergy(float energyCost)
        {
            if (IsEnoughtEnergy(energyCost))
            {
                currentEnergy -= energyCost;
                return true;
            }

            return false;   
        }
        
        public bool IsEnoughtEnergy(float energyCost)
        {
            return currentEnergy >= energyCost;
        }

        public void ChangeActiveState()
        {
            Active = !Active;
            
            if (!Active)
            {
                StartCoroutine(RestoreEnergyAsync());
            }
        }

        private IEnumerator RestoreEnergyAsync()
        {
            while (!Active && currentEnergy < maxEnergy)
            {
                currentEnergy += restorePerTick;
                yield return new WaitForSeconds(restorePerTick);
            }

            currentEnergy = Mathf.Min(currentEnergy, maxEnergy);
        }
    }
}