using System;

namespace Interfaces
{
    public interface IFighter
    {
        public event Action<IFighter> OnFightStartEvent;
        public event Action<IFighter> OnFightEndEvent;
    }
}