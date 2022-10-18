using System;
using Interfaces;
using UnityEngine;

namespace Stats.Health
{
    public class EnemyHealthView : MonoBehaviour, IHaveHealth
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private GameObject mainObjects;
        private EnemyHealth health;

        private void Start()
        {
            health = new EnemyHealth(maxHealth);
            health.OnDeathEvent += Disable;
        }

        private void OnDisable()
        {
            health.OnDeathEvent -= Disable;
        }

        private void Disable()
        {
            mainObjects.SetActive(false);
        }
        
        public BaseHealth GetHealth()
        {
            return health;
        }
    }
}