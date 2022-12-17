using Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    public class UnitWithMovement
    {
        private NavMeshAgent agent;
        private Vector3[] patrolPoints;
        private float moveSpeed;
        private Transform playerTransform;

        private FightingUnit fightingState;
        
        private Vector3 currentPoint;

        private bool isChasingHero;

        public UnitWithMovement(NavMeshAgent agent, Vector3[] patrolPoints, float moveSpeed, Transform playerTransform, FightingUnit fightingUnit)
        {
            this.agent = agent;
            this.patrolPoints = patrolPoints;
            this.moveSpeed = moveSpeed;
            this.playerTransform = playerTransform;
            fightingState = fightingUnit;
            fightingState.OnFightStartEvent += StartChase;
            fightingState.OnFightEndEvent += EndChase;
        }

        public void UpdateMovement()
        {
            if (DidReachPoint() && !isChasingHero)
            {
                DetermineNewDestinationPoint();
            }
            else if (isChasingHero)
            {
                ChaiseHero();
            }
        }

        private void StartChase(IFighter fighter)
            => isChasingHero = true;
        
        private void EndChase(IFighter fighter)
            => isChasingHero = false;
        
        
        private bool DidReachPoint()
        {
            return agent.pathStatus == NavMeshPathStatus.PathComplete;
        }
        
        private void DetermineNewDestinationPoint()
        {
            Move(true);
            SetSpeed(moveSpeed);
            var randomPointIndex = Random.Range(0, patrolPoints.Length);
            var randomPoint = patrolPoints[randomPointIndex];
            SetDestination(randomPoint);
        }

        private void ChaiseHero()
        {
            Move(true);
            SetSpeed(moveSpeed);
            SetDestination(playerTransform.position);
        }
        
        private void Move(bool value)
        {
            agent.isStopped = !value;
        }
        
        private void SetSpeed(float value)
        {
            agent.speed = value;
        }

        private void SetDestination(Vector3 destination)
        {
            agent.SetDestination(destination);
        }
    }
}