
public interface IUseLight
{
    public UnitLight Light { get; }
    
    public void SetLight(UnitLight light);
    public void ScaleLight(float scaleVal);
    public void ScaleLightUp();
    public void ScaleLightUpOverTime(float time);
    public void ScaleLightDown();
    public void ScaleLightDownOverTime(float time);
    public void SetLight(bool state);
}
