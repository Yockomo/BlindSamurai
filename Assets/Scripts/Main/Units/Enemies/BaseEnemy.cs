using Interfaces;
using Interfaces.Pause_Interfaces;
using ScriptableObjects.Enemies;
using Stats.Health;
using Units;
using UnityEngine;

namespace Enemies
{
    public class BaseEnemy : MonoBehaviour, IPausable, IHaveHealth
    {
        public bool IsPaused { get; private set;}
                
        protected FightingUnit fightingUnit;
        protected UnitWithLight unitWithLight;
        protected EnemyHealth enemyHealth;
        
        public virtual void Construct(Transform targetTransform, EnemySettings config,
            IPausableUnitsRegisterService pausableUnitsRegisterService, IFightingStateService fightingStateService)
        {
            fightingUnit = new FightingUnit(transform, targetTransform, config.FightingDistance, fightingStateService);
            
            unitWithLight = new UnitWithLight(config.UnitLight, transform);
            
            enemyHealth = new EnemyHealth(config.MaxHealthPoints);
            ConfigureHealthEvent();
            
            pausableUnitsRegisterService.Register(this);
        }

        private void OnDestroy()
        {
            enemyHealth.OnDeathEvent -= OffObject;
            enemyHealth.OnDeathEvent -= fightingUnit.Disable;
        }

        private void Update()
        {
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
            fightingUnit.CheckFightingState();
        }
        
        public void SetPauseState(bool stateValue)
        {
            IsPaused = stateValue;
        }

        public BaseHealth GetHealth()
        {
            return enemyHealth;
        }

        protected void ConfigureHealthEvent()
        {
            enemyHealth.OnDeathEvent += OffObject;
            enemyHealth.OnDeathEvent += fightingUnit.Disable;
        }

        protected void OffObject()
        {
            gameObject.SetActive(false);
        }
    }
}