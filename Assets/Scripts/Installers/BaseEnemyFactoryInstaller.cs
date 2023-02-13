using Interfaces;
using Interfaces.Pause_Interfaces;
using Main.Infrastructure.Factories;
using ScriptableObjects.Enemies;
using UnityEngine;
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
            //TODO extract 
            Container.Bind<Transform>().FromInstance(playerTransform).AsSingle();
            
            var baseFactory = new EnemyFactories(enemyConfig, playerTransform, pausableService, fightingService);
            Container.Bind<EnemyFactories>().FromInstance(baseFactory).AsSingle();
        }
    }
}