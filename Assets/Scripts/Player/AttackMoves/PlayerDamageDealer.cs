using Systems;
using UnityEngine;

namespace Player.AttackMoves
{
    public class PlayerDamageDealer : MonoBehaviour
    {
        [SerializeField] private LayerMask layerToDamage;
        [SerializeField] private int damage;
        [SerializeField] private Transform positionToHit;
        [SerializeField] private float circleRadius;
        [SerializeField] private Vector2 direction;
        [SerializeField] private float distance;
        [SerializeField] private int maxDamageUnitsPerHit;
        [SerializeField] private float attackSpeed;
        
        [Header("References")]
        [SerializeField] private PlayerStates playerStates;
        
        private DamageDealer damageDealer;
        private float nextAttackTime = 2f;
        
        private void Start()
        {
            damageDealer = new DamageDealer(layerToDamage, maxDamageUnitsPerHit);
        }

        private void Update()
        {
            if (playerStates.IsAttacking && Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackSpeed;
            }
        }        
        
        private void Attack()
        {
            if (damageDealer.TryDetectCircleHit(positionToHit.position, circleRadius, direction, distance, out var result))
            {
                damageDealer.ApplyDamageTo(damage, result);
            }
        }

        private void OnDrawGizmos()
        {
            if(positionToHit == null)
                return;
            
            Gizmos.DrawWireSphere(positionToHit.position, circleRadius);
        }
    }
}