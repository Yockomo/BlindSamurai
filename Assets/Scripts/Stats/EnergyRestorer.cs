using System.Collections;
using Player;
using UnityEngine;
using Zenject;

namespace Stats
{
    public class EnergyRestorer : MonoBehaviour
    {
        [SerializeField] private PlayerStates playerStates;

        private Energy playersEnergy;
        private float restorePerTick;
        private bool isRestoring;
        
        [Inject]
        private void Construct(Energy energy)
        {
            playersEnergy = energy;
            restorePerTick = playersEnergy.RestorePerTick;
        }

        private void Update()
        {
            if (ValidFightState() && !isRestoring)
            {
                StartCoroutine(RestoreEnergy());
            }
        }
        
        private IEnumerator RestoreEnergy()
        {
            isRestoring = true;
            
            while (ValidFightState() && !playersEnergy.IsMaxEnergy())
            {
                playersEnergy.RestoreEnergy(restorePerTick);
                yield return new WaitForSeconds(1);
            }

            isRestoring = false;
        }
        
        private bool ValidFightState()
        {
            return playerStates.Inactive;
        }
    }
}