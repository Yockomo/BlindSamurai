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
            
            ConfigureEvent();
            
            pausableUnitsRegisterService.Register(this);
        }

        private void OnDestroy()
        {
            enemyHealth.OnDeathEvent -= OffObject;
            enemyHealth.OnDeathEvent -= fightingUnit.Disable;
            fightingUnit.OnFightStartEvent -= LightUp;
            fightingUnit.OnFightEndEvent -= LightDown;
        }

        protected void ConfigureEvent()
        {
            enemyHealth.OnDeathEvent += OffObject;
            enemyHealth.OnDeathEvent += fightingUnit.Disable;
            fightingUnit.OnFightStartEvent += LightUp;
            fightingUnit.OnFightEndEvent += LightDown;
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

        protected void OffObject()
        {
            gameObject.SetActive(false);
        }

        protected void LightUp(IFighter fighter) 
        {
            unitWithLight.LightUp();
            Debug.Log(gameObject.name + " light up");
        }

        protected void LightDown(IFighter fighter) 
        {
            unitWithLight.LightDown();
            Debug.Log(gameObject.name + " light down");
        }
    }
}