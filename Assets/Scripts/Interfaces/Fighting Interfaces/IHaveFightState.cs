namespace Interfaces
{
    public interface IHaveFightState
    {
        public bool IsFighting { get; }
    
        public void SetFighState(bool stateValue);
    }
}