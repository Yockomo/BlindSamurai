using UnityEngine;

namespace Animations
{
    public abstract class BaseAnimatorManager
    {
        protected readonly Animator animator;
        
        public BaseAnimatorManager(Animator animator)
        {
            this.animator = animator;
        }
        
        protected void SetBool(int parameter, bool value)
        {
            animator.SetBool(parameter, value);
        }
        
        protected void SetFloat(int parameter, float value)
        {
            animator.SetFloat(parameter, value);
        }
        
        protected void SetInteger(int parameter, int value)
        {
            animator.SetInteger(parameter,value);
        }
        
        protected void SetTrigger(int parameter)
        {
            animator.SetTrigger(parameter);
        }
        
        protected void ResetTrigger(int parameter)
        {
            animator.ResetTrigger(parameter);
        }
    }
}