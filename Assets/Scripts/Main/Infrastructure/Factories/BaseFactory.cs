using Enemies;
using Interfaces;
using Interfaces.Pause_Interfaces;
using ScriptableObjects.Enemies;
using UnityEngine;

public abstract class BaseFactory<T> where T : BaseEnemy
{
    protected EnemyConfiguration enemyConfig;
    protected Transform playerTransform;
    protected IPausableUnitsRegisterService pausableService;
    protected IFightingStateService fightingService;
    
    public BaseFactory(EnemyConfiguration enemyConfig, Transform playerTransform, 
        IPausableUnitsRegisterService pausableService, IFightingStateService fightingService)
    {
        this.enemyConfig = enemyConfig;
        this.playerTransform = playerTransform;
        this.pausableService = pausableService;
        this.fightingService = fightingService;
    }
    
    public T CreateEnemy(int enemyId)
    {
        var enemySettings = enemyConfig.GetEnemyConfig(enemyId);
        var enemy = Object.Instantiate(enemySettings.EnemyModel);
        var baseEnemy = enemy.AddComponent<T>();
        baseEnemy.Construct(playerTransform, enemySettings, pausableService, fightingService);
        
        return baseEnemy;
    }
}
