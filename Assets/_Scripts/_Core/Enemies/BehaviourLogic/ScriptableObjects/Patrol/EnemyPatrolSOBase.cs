using UnityEngine;
using UnityEngine.AI;
using WormGame.Core.Utils;

namespace WormGame.Core.Enemies
{
    public class EnemyPatrolSOBase : ScriptableObject
    {
        protected EnemyChaseState _chaseState;
        protected EnemyPatrollingState _patrolState;
        protected EnemyAwarenessController _enemyAwarenessController;
        protected float _changeDirectionCooldown = 5;
        protected Vector2 _target;
        protected CompositeCollider2D _spawnAreaCollider;
        protected NavMeshAgent _agent;
        public EnemyChaseState ChaseState => _chaseState;
        public Vector2 Target => _target;

        public virtual void Initialize(EnemyChaseState chaseState, EnemyPatrollingState patrolState,
            EnemyAwarenessController enemyAwarenessController,
            NavMeshAgent agent)
        {
            _chaseState = chaseState;
            _patrolState = patrolState;
            _enemyAwarenessController = enemyAwarenessController;
            _agent = agent;
            _spawnAreaCollider = GameObject.FindWithTag("SpawnArea").GetComponent<CompositeCollider2D>();

        }

        public virtual EnemyState RunCurrentState()
        {
            return RunCurrentState(_target);
        }

        public virtual EnemyState RunCurrentState(Vector2 _target)
        {
            if (_enemyAwarenessController.CanSeePlayer)
            {
                return _chaseState;
            }

            else
            {
                if (_changeDirectionCooldown >= 0)
                {
                    _changeDirectionCooldown -= Time.deltaTime;
                }

                else
                {
                    _target = SpawnUtils.GetRandomPointInCompositeCollider(in _spawnAreaCollider);
                    _agent.SetDestination(_target);

                    _changeDirectionCooldown = Random.Range(5, 8);
                }

                return _patrolState;
            }
        }
    }
}

