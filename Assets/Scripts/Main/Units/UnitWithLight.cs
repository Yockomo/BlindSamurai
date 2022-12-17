using Interfaces;
using UnityEngine;

namespace Units
{
    public class UnitWithLight
    {
        private Transform selfTransform;
        private UnitLight unitLight;
        
        public UnitWithLight(UnitLight lights, Transform selfTransform, FightingUnit fightingUnit)
        {
            var light = Object.Instantiate(lights);
            unitLight = light;
            SetUnitLight(selfTransform);
            TurnLight(true);
            
            fightingUnit.OnFightStartEvent += ScaleLightUp;
            fightingUnit.OnFightEndEvent += ScaleLightDown;
        }

        private void SetUnitLight(Transform transform)
        {
            var lightTransform = unitLight.transform;
            lightTransform.SetParent(transform);
            lightTransform.localPosition = Vector3.zero;
        }

        public void TurnLight(bool value)
        {
            if (value)
                unitLight.TurnOn();
            else
                unitLight.TurnOff();
        }
        
        private void ScaleLightUp(IFighter fighter)
        {
            unitLight.ScaleUp();
        }        
        
        private void ScaleLightDown(IFighter fighter)
        {
            unitLight.ScaleDown();
        }
    }
}