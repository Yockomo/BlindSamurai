using Interfaces;
using UnityEngine;

namespace Stats.Health
{
    public class EnemyHealthView : MonoBehaviour, IHaveHealth
    {
        [SerializeField] private int maxHealth;
        private EnemyHealth health;

        private void Start()
        {
            health = new EnemyHealth(maxHealth);
        }

        public BaseHealth GetHealth()
        {
            return health;
        }
    }
}