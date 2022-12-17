using System.Collections.Generic;
using Interfaces.Pause_Interfaces;
using UnityEngine;

namespace Services
{
    public class PausableUnitsService : IPausableUnitsRegisterService
    {
        private List<IPausable> units;

        public PausableUnitsService()
        {
            units = new List<IPausable>();
        }
        
        public void Register(IPausable pausableUnit)
        {
            if (!units.Contains(pausableUnit))
            {
                units.Add(pausableUnit);
            }
            else
            {
                Debug.LogError($"Tried to add unit {pausableUnit} that already exist in list");
            }
        }

        public void Pause()
        {
            foreach (var unit in units)
            {
                unit.SetPauseState(true);
            }
        }

        public void Unpause()
        {
            foreach (var unit in units)
            {
                unit.SetPauseState(false);
            }
        }
    }
}