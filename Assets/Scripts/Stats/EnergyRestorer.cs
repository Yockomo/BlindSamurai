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
            if (ValidState() && !isRestoring)
            {
                StartCoroutine(RestoreEnergy());
            }
        }
        
        private IEnumerator RestoreEnergy()
        {
            isRestoring = true;
            
            while (ValidState() && !playersEnergy.IsMaxEnergy())
            {
                playersEnergy.RestoreEnergy(restorePerTick);
                yield return new WaitForSeconds(restorePerTick);
            }

            isRestoring = false;
        }

        private bool ValidState()
        {
            return playerStates.IsFighting && playerStates.Inactive;
        }
    }
}