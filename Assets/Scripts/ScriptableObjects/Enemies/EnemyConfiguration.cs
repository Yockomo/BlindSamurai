using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Enemies
{
    [CreateAssetMenu(fileName = "EnemyConfiguration", menuName = "ScriptableObjects/Enemy/EnemyConfig", order = 0)]
    public class EnemyConfiguration : ScriptableObject
    {
        [SerializeField] private EnemySettings[] settings;
        
        private Dictionary <int, EnemySettings> enemies;
        
        public EnemySettings GetEnemyConfig(int enemyId)
        {
            if (enemies == null)
                FillDictionary();

            return enemies[enemyId];
        }

        private void FillDictionary()
        {
            enemies = new Dictionary<int, EnemySettings>(settings.Length);
            
            foreach (var enemy in settings)
            {
                if (!enemies.ContainsKey(enemy.EnemyId))
                    enemies.Add(enemy.EnemyId, enemy);
            }
        }
    }
    
    [Serializable]
    public struct EnemySettings
    {
        public int EnemyId;
        public GameObject EnemyModel;
        public UnitLight UnitLight;        
        public float FightingDistance;
        public int MaxHealthPoints;
        public float MoveSpeed;
        public float StopDistance;
    }
}