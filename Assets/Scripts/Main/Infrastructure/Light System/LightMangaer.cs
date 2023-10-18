using UnityEngine;

namespace Main.Infrastructure.Light_System
{
    public class LightMangaer : MonoBehaviour, IUseLight
    {
        public UnitLight Light { get; private set; }

        public void SetLight(UnitLight light)
        {
            Light = light;
            var lightTransform = Light.transform;
            lightTransform.SetParent(transform);
            lightTransform.localPosition = Vector3.zero;
        }

        public void ScaleLight(float scaleVal)
        {
            Light.Scale(scaleVal);
        }

        public void ScaleLightUp()
        {
            Light.ScaleUp();
        }
        
        public void ScaleLightUpOverTime(float time)
        {
            Light.ScaleUp(time);
        }

        public void ScaleLightDown()
        {
            Light.ScaleDown();
        }

        public void ScaleLightDownOverTime(float time)
        {
            Light.ScaleDown(time);
        }
        
        public void SetLight(bool state)
        {
            if(state)
                Light.TurnOn();
            else
                Light.TurnOff();
        }
    }
}