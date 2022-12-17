
using System;
using Interfaces;
using Interfaces.Pause_Interfaces;
using ScriptableObjects.Enemies;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class Slime : BaseEnemy
    {
        [SerializeField] private Transform targetTransform;
        [SerializeField] private EnemyConfiguration enemyConfig;
        private IPausableUnitsRegisterService pausableService;
        private IFightingStateService fightingStateService;
        
        [Inject]
        private void Construct(IPausableUnitsRegisterService pausableUnitsRegisterService, IFightingStateService fightingStateService)
        {
            pausableService = pausableUnitsRegisterService;
            this.fightingStateService = fightingStateService;
        }
        
        private void Start()
        {
            Construct(targetTransform,enemyConfig.enemySettings, pausableService, fightingStateService);
        }
    }
}