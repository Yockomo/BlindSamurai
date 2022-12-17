using UnityEngine;

namespace Units
{
    public class UnitWithLight
    {
        private Transform selfTransform;
        private UnitLight unitLight;
        
        public UnitWithLight(UnitLight lights, Transform selfTransform)
        {
            var light = Object.Instantiate(lights);
            unitLight = light;
            SetUnitLight(selfTransform);
            TurnLight(true);
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
    }
}