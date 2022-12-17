using UnityEngine;

namespace Animations
{
    public class PlayerAnimatorManager : BaseAnimatorManager
    {
        private readonly int attack = Animator.StringToHash("Attack");
        
        public PlayerAnimatorManager(Animator animator) : base(animator)
        {
        }

        public void AttackAnimation()
        {
            SetBool(attack,true);
        }
    }
}