using System;
using Interfaces;
using Interfaces.Pause_Interfaces;
using Units;
using UnityEngine;

namespace Enemies
{
    public class BaseEnemy : MonoBehaviour, IPausable
    {
        public bool IsPaused { get; private set;}
                
        private FightingUnit fightingUnit;
        private UnitWithLight unitWithLight;
                
        public void Construct(Transform targetTransform, float fightingDistance, IFightingStateService fightingStateService,
            UnitLight unitLight,
            IPausableUnitsRegisterService pausableUnitsRegisterService)
        {
            fightingUnit = new FightingUnit(transform, targetTransform, fightingDistance, fightingStateService);
            unitWithLight = new UnitWithLight(unitLight, transform);
            pausableUnitsRegisterService.Register(this);
        }

        private void Update()
        {
            fightingUnit.CheckFightingState();
        }

        public void SetPauseState(bool stateValue)
        {
            IsPaused = stateValue;
        }
    }
}