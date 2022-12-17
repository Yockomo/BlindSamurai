using System;
using Enemies;
using Interfaces;
using Interfaces.Pause_Interfaces;
using Main.Infrastructure.Factories;
using ScriptableObjects.Enemies;
using UnityEngine;
using UnityEngine.Timeline;
using Zenject;

namespace Installers
{
    //TODO Looks like a shit
    public class BaseEnemyFactoryInstaller : MonoInstaller
    {
        [SerializeField] private EnemyConfiguration enemyConfig;
        [SerializeField] private Transform playerTransform;

        private IPausableUnitsRegisterService pausableService;
        private IFightingStateService fightingService;

        [Inject]
        private void Construct(IPausableUnitsRegisterService pausableService, IFightingStateService fightingService)
        {
            this.pausableService = pausableService;
            this.fightingService = fightingService;
        }
        
        public override void InstallBindings()
        {
            var baseFactory = new EnemyFactories(enemyConfig, playerTransform, pausableService, fightingService);
            Container.Bind<EnemyFactories>().FromInstance(baseFactory).AsSingle();
        }
    }
}