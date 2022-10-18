using UnityEngine;

namespace Stats.Health
{
    public class EnemyHealth : BaseHealth
    {
        public EnemyHealth(int maxHealth) : base(maxHealth)
        {
        }

        public override void TakeDamage(int damageValue)
        {
            base.TakeDamage(damageValue);
            Debug.Log($"Damaged with {damageValue} damage and current health = {CurrentHealth} hp");
        }
    }
}