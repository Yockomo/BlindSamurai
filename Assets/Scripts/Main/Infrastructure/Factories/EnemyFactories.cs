using Enemies;
using Interfaces;
using Interfaces.Pause_Interfaces;
using ScriptableObjects.Enemies;
using UnityEngine;

namespace Main.Infrastructure.Factories
{
    public class EnemyFactories: BaseFactory<BaseEnemy>
    {
        public EnemyFactories(EnemyConfiguration enemyConfig, Transform playerTransform, IPausableUnitsRegisterService pausableService, IFightingStateService fightingService) : base(enemyConfig, playerTransform, pausableService, fightingService)
        {
        }
    }
}