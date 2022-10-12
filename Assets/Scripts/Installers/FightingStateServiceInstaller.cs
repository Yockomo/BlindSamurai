using Interfaces;
using Services;
using Zenject;

namespace Installers
{
    public class FightingStateServiceInstaller : MonoInstaller
    {
        public IHaveFightState heroStates;
        public int startEnemyCount;
        
        public override void InstallBindings()
        { 
            var fightingStateService = new FightingStateService(heroStates,startEnemyCount);
            
            Container.Bind<IFightingStateService>().FromInstance(fightingStateService).AsSingle().NonLazy();
        } 
    }
}