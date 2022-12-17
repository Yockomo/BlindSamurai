using Main.Infrastructure.Factories;
using UnityEngine;
using Zenject;

namespace Main.Infrastructure
{
    public class EnemySpawnMarker : MonoBehaviour
    {
        [SerializeField] private int enemyId;

        [Inject]
        private void Construct(EnemyFactories enemyFactories)
        {
            enemyFactories.CreateEnemy(enemyId);
        }
    }
}