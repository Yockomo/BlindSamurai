using System;
using Interfaces;
using Interfaces.Pause_Interfaces;
using Player;
using UnityEngine;
using Zenject;

namespace Units
{
    public class Unit : MonoBehaviour, IPausable, IFighter
    {
        public bool IsPaused { get; private set; }
        
        public event Action<IFighter> OnFightStartEvent;
        public event Action<IFighter> OnFightEndEvent;

        private bool inFight;
        
        [Inject]
        private void Construct(IFightingStateService fightingService, IPausableUnitsRegisterService pausableService)
        {
            fightingService.RegisterFighter(this);
            pausableService.Register(this);
        }

        private void OnDisable()
        {
            OnFightEndEvent?.Invoke(this);
            if(inFight)
                inFight = false;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<PlayerStates>(out var playerStates) && inFight)
            {
                inFight = false;
                OnFightEndEvent?.Invoke(this);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<PlayerStates>(out var playerStates) && !inFight)
            {
                inFight = true;
                OnFightStartEvent?.Invoke(this);
            }
        }

        public void SetPauseState(bool stateValue)
        {
            IsPaused = stateValue;
        }
    }
}