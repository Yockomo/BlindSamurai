using System;
using Interfaces;
using UnityEngine;

namespace Units
{
    public class FightingUnit : IFighter
    {        
        public bool InFight { get; private set; }   
        
        public event Action<IFighter> OnFightStartEvent;
        public event Action<IFighter> OnFightEndEvent;

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
            
            if (distance < figthingDistance && !InFight)
            {
                 InFight = true;
                 OnFightStartEvent?.Invoke(this);
            }
            else if(distance > figthingDistance && InFight)
            {
                 InFight = false;
                 OnFightEndEvent?.Invoke(this);
            }
        }

        public void Disable()
        {
            InFight = false;
            OnFightEndEvent?.Invoke(this);
        }
    }
}