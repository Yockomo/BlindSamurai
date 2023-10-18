using Interfaces;
using Interfaces.Pause_Interfaces;
using Main.Infrastructure.Light_System;
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
        protected EnemyHealth enemyHealth;
        
        public virtual void Construct(Transform targetTransform, EnemySettings config,
            IPausableUnitsRegisterService pausableUnitsRegisterService, IFightingStateService fightingStateService)
        {
            fightingUnit = new FightingUnit(transform, targetTransform, config.FightingDistance, fightingStateService);
            enemyHealth = new EnemyHealth(config.MaxHealthPoints);

            ConfigureLight(config);
            ConfigureEvent();
            
            pausableUnitsRegisterService.Register(this);
        }

        private void OnDestroy()
        {
            enemyHealth.OnDeathEvent -= OffObject;
            enemyHealth.OnDeathEvent -= fightingUnit.Disable;
        }

        protected void ConfigureLight(EnemySettings config)
        {
            var light = Instantiate(config.UnitLight);
            if(TryGetComponent<LightMangaer> (out LightMangaer manager))
                manager.SetLight(light);
        }
        
        protected void ConfigureEvent()
        {
            enemyHealth.OnDeathEvent += OffObject;
            enemyHealth.OnDeathEvent += fightingUnit.Disable;
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
    }
}