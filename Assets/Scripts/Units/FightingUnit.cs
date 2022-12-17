using System;
using Interfaces;
using UnityEngine;

namespace Units
{
    //TODO Rename class name to FightingUnit, Extract IPausable interface to head class,
    //change logic from trigger to Vector3.Distance(this.transorm.position, player.transform.position)
    public class FightingUnit : IFighter
    {
        public event Action<IFighter> OnFightStartEvent;
        public event Action<IFighter> OnFightEndEvent;

        private bool inFight;        
        
        private Transform selftTransform;
        private Transform targetTransform;
        private float figthingDistance;
        
        public FightingUnit(Transform selftTransform, Transform targetTransform, float figthingDistance, IFightingStateService fightingService)
        {
            this.selftTransform = selftTransform;
            this.targetTransform = targetTransform;
            this.figthingDistance = figthingDistance;
            fightingService.RegisterFighter(this);
        }

        public void CheckFightingState()
        {
            var distance = Vector3.Distance(selftTransform.position, targetTransform.position);
            
            if (distance < figthingDistance && !inFight)
            {
                 inFight = true;
                 OnFightStartEvent?.Invoke(this);
            }
            else if(distance > figthingDistance && inFight)
            {
                 inFight = false;
                 OnFightEndEvent?.Invoke(this);
            }
        }
    }
}