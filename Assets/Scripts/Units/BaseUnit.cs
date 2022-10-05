using UnityEngine;

namespace Units
{
    public class BaseUnit : MonoBehaviour
    {
        [SerializeField] private UnitLight lights;
        
        private UnitLight unitLight;

        private void OnEnable()
        {
            var light = Instantiate(lights);
            SetUnitLight(light);
        }

        private void OnDisable()
        {
            ResetUnitLight();
        }

        public void SetUnitLight(UnitLight light)
        {
            unitLight = light;
            unitLight.transform.SetParent(transform);
            unitLight.transform.localPosition = Vector3.zero;
            unitLight.TurnOn();
        }

        private void ResetUnitLight()
        {
            unitLight.TurnOff();
            unitLight = null;
        }
    }
}