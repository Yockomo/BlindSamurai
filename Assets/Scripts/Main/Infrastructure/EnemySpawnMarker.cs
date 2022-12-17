using Main.Infrastructure.Factories;
using UnityEngine;
using Zenject;

namespace Main.Infrastructure
{
    public class EnemySpawnMarker : MonoBehaviour
    {
        [SerializeField] private int enemyId;
        [SerializeField] private Transform parentTo;
        
        [Inject]
        private void Construct(EnemyFactories enemyFactories)
        {
            var enemy = enemyFactories.CreateEnemy(enemyId);
            enemy.transform.parent = parentTo;
            enemy.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}