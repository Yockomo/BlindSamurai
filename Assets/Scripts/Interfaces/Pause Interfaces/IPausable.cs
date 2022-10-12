namespace Interfaces.Pause_Interfaces
{
    public interface IPausable
    {
        public bool IsPaused { get; }
        public void SetPauseState(bool stateValue);
    }
}