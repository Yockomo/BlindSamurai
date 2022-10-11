using System;

namespace Interfaces
{
    public interface IUiElement<T> 
    {
        public event Action<T> OnValueChange;
    }
}