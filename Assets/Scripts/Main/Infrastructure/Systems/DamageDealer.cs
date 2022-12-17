using Interfaces;
using UnityEngine;

namespace Systems
{
    public class DamageDealer
    {
        private LayerMask layerToDamage;
        private int maxUnitsToHit;
        
        public DamageDealer(LayerMask layerToDamage, int maxUnitsToHit)
        {
            this.layerToDamage = layerToDamage;
            this.maxUnitsToHit = maxUnitsToHit;
        }
        
        public bool TryDetectCircleHit(Vector2 position, float circleRadius, Vector2 direction, float distance, out RaycastHit2D[] result)
        {
            result = new RaycastHit2D[maxUnitsToHit];
            Physics2D.CircleCastNonAlloc(position, circleRadius, direction, result, distance ,layerToDamage);
            
            return result.Length > 0;
        }
        
        public bool TryApplyDamageTo(int damage, Collider2D unitCollider)
        {
            if (unitCollider != null && unitCollider.TryGetComponent<IHaveHealth>(out var health))
            {
                ApplyDamageTo(damage, health.GetHealth());
                return true;
            }

            return false; 
        }
        
        public void ApplyDamageTo(int damage, Collider2D[] units)
        {
            foreach (var unit in units)
            {
                TryApplyDamageTo(damage, unit);
            }
        }
        
        public void ApplyDamageTo(int damage, RaycastHit2D[] raycastHits)
        {
            if (raycastHits.Length > 0)
            {
                foreach (var raycast in raycastHits)
                {
                    TryApplyDamageTo(damage, raycast.collider);
                }
            }
        }
        
        public void ApplyDamageTo(int damage,BaseHealth health)
        {
            health.TakeDamage(damage);
        }
    }
}