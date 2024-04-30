using UnityEngine;
using UnityEngine.AI;

namespace WormGame.Core.Enemies
{
    public class EnemyChaseSOBase : ScriptableObject
    {
        protected EnemyChaseState _chaseState;
        protected EnemyPatrollingState _patrolState;
        protected EnemyAwarenessController _enemyAwarenessController;
        protected Transform _target;
        protected Transform _transform;
        protected NavMeshAgent _agent;

        public virtual void Initialize(EnemyChaseState chaseState, EnemyPatrollingState patrolState, Transform transform, Transform m_Target,
            EnemyAwarenessController enemyAwarenessController,
            NavMeshAgent agent)
        {
            _transform = transform;
            _chaseState = chaseState;
            _patrolState = patrolState;
            _target = m_Target;
            _enemyAwarenessController = enemyAwarenessController;
            _agent = agent;
        }

        public virtual EnemyState RunCurrentState()
        {
            if (_enemyAwarenessController.CanSeePlayer)
            {
                _agent.SetDestination(_target.position);

                return _chaseState;
            }

            else
            {
                return _patrolState;
            }
        }
    }
}
