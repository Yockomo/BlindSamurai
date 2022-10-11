using ScriptableObjects;
using Stats;
using Zenject;

namespace Installers
{
    public class EnergyInstaller : MonoInstaller
    {
        public PlayerEnergyData EnergyData;
        public override void InstallBindings()
        { 
            var energy = new Energy(EnergyData.maxEnergy, EnergyData.energyRestoreSpeedInSeconds);
            
            Container.Bind<Energy>().FromInstance(energy).AsSingle().NonLazy();
        } 
    }
}