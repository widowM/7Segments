using UnityEngine;
using UnityEngine.AI;
using WormGame.EventChannels;

namespace WormGame.Core.Enemies
{
    public class EnemyChaseState : EnemyState
    {
        [SerializeField] protected WormDataSO _wormDataSO;
        protected Transform _target;
        [SerializeField] protected NavMeshAgent _agent;
        [SerializeField] protected EnemyAwarenessController _enemyAwarenessController;
        public EnemyPatrollingState patrollingState;
        public EnemyChaseSOBase enemyChaseSOBase;

        [SerializeField] private VoidEventChannelSO _wormReadySO;

        public EnemyChaseSOBase EnemyChaseSOBaseInstance { get; set; }

        private void Init()
        {
            _target = _wormDataSO.Head.transform;
            EnemyChaseSOBaseInstance = Instantiate(enemyChaseSOBase);
            EnemyChaseSOBaseInstance.Initialize(this, patrollingState, transform, _target, _enemyAwarenessController, _agent);
        }

        public override EnemyState RunCurrentState()
        {
            return EnemyChaseSOBaseInstance.RunCurrentState();
        }

        private void OnEnable()
        {
            _wormReadySO.OnEventRaised += Init;
        }

        private void OnDisable()
        {
            _wormReadySO.OnEventRaised -= Init;
        }
    }
}