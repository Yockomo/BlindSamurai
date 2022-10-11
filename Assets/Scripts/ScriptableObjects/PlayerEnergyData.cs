using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerEnergyData", menuName = "ScriptableObjects/Energy/PlayerEnergyCost", order = 1)]
    public class PlayerEnergyData : ScriptableObject
    {
        [Header("Energy settings")]
        public float maxEnergy;
        public float energyRestoreSpeedInSeconds;
        
        [Header("Energy cost")]

        [Header("Jump cost"), Space(5)]
        public int defaultJumpCost;
        public int wallJumpCost;
        
        [Header("Dash cost"), Space(5)]
        public int dashCost;
        public int airDashCost;
        
        [Header("Attack cost"), Space(5)]
        public int defaultAttackCost;
        public int blockedAttackCost;
        public int airAttackCost;
        public int airDownAttackCost;
        public int airDiagonalAttackCost;
    }
}