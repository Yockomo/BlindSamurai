using System;
using UnityEngine;

namespace ScriptableObjects.Enemies
{
    [CreateAssetMenu(fileName = "EnemyConfiguration", menuName = "ScriptableObjects/Enemy/EnemyConfig", order = 0)]
    public class EnemyConfiguration : ScriptableObject
    {
        [Header("Settings")] 
        public EnemySettings enemySettings;
    }
    
    [Serializable]
    public struct EnemySettings
    {
        public float FightingDistance;
        public UnitLight UnitLight;
        public int MaxHealthPoints;
    }
}