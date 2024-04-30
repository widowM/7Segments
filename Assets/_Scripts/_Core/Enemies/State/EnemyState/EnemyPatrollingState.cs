using UnityEngine;
using UnityEngine.AI;
using WormGame.Core.Utils;

namespace WormGame.Core.Enemies
{
    public class EnemyPatrollingState : EnemyState
    {
        public EnemyChaseState chaseState;
        protected CompositeCollider2D _spawnAreaCollider;
        protected float _changeDirectionCooldown = 5;
        protected float _changeDirectionCooldownMin = 1;
        protected float _changeDirectionCoolDownMax = 8;
        [SerializeField] protected NavMeshAgent _agent;
        [SerializeField] protected EnemyAwarenessController _enemyAwarenessController;
        [SerializeField] protected LayerMask _spawnMask;
        public EnemyPatrolSOBase enemyPatrolSOBase;
        protected Vector3 _target;

        public EnemyPatrolSOBase EnemyPatrolSOBaseInstance { get; set; }
        private void Awake()
        {
            EnemyPatrolSOBaseInstance = Instantiate(enemyPatrolSOBase);
            EnemyPatrolSOBaseInstance.Initialize(chaseState, this, _enemyAwarenessController, _agent);
        }

        protected void Start()
        {

            _target = Vector3.zero;
        }

        public override EnemyState RunCurrentState()
        {
            return EnemyPatrolSOBaseInstance.RunCurrentState();
        }
    }
}
