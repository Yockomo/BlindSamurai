using Interfaces.Pause_Interfaces;
using Services;
using Zenject;

namespace Installers
{
    public class PausableUnitsRegisterServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var service = new PausableUnitsService();
            
            Container.Bind<IPausableUnitsRegisterService>().FromInstance(service).AsSingle().NonLazy();
        }
    }
}