using UnityEngine;
using UnityEngine.AI;
using WormGame.EventChannels;
using WormGame.Core.Utils;

namespace WormGame.Core.Enemies
{
    [CreateAssetMenu(fileName = "Diagey Chase Enemy Behaviour", menuName = "Enemy Behaviour/Chase Logic/Diagey Chase")]
    public class EnemyDiageyChaseSO : EnemyChaseSOBase
    {
        private Rigidbody2D _rb2D;
        private bool _hasCharged = false;
        [SerializeField] private AudioClip[] _chargingSFXVariations;

        // A timer to delay the charge and make the charging action available again
        private CustomTimer _chargeTimer;
        [SerializeField] private Vector2EventChannelSO m_EnemyReadyToAttackSO;

        public override void Initialize(EnemyChaseState chase, EnemyPatrollingState patrol, Transform transform, Transform target,
            EnemyAwarenessController enemyAwareness, NavMeshAgent agent)
        {
            base.Initialize(chase, patrol, transform, target, enemyAwareness, agent);

            Transform grandGrandParentTransform = _transform.parent.transform.parent.transform.parent;
            _rb2D = grandGrandParentTransform.GetComponent<Rigidbody2D>();
            _chargeTimer = _transform.GetComponent<CustomTimer>();
            _hasCharged = false;
        }

        public override EnemyState RunCurrentState()
        {
            if (_enemyAwarenessController.CanSeePlayer)
            {
                // If the enemy has not charged yet, call the Charge method
                if (!_hasCharged)
                {
                    _agent.speed = 0;
                    _agent.SetDestination(_target.position); 

                    // Set the charge flag to true
                    _hasCharged = true;
       
                    StartCharging();
                }

                return _chaseState;
            }

            else
            {
                float defaultAgentSpeed = 1.2f;
                _agent.speed = defaultAgentSpeed;
                _hasCharged = false;

                return _patrolState;
            }

        }

        // A method to start the charging behavior
        public void StartCharging()
        {
            AudioClip chargingSFX = _chargingSFXVariations[Random.Range(0, _chargingSFXVariations.Length)];
            AudioSource.PlayClipAtPoint(chargingSFX, _transform.position, 1f);
            
            // Get the current position of the player head
            Vector2 currentHeadPosition = _target.position;

            // Get the direction to the player head
            Vector2 chargeDirection = (currentHeadPosition - (Vector2)_transform.position).normalized;

            //Debug.DrawLine(currentHeadPosition, _transform.position, Color.red, 4);
            OnEnemyReadyToAttack();

            // Call the back off method
            BackOff(chargeDirection);
        }

        private void OnEnemyReadyToAttack()
        {
            if (_enemyAwarenessController.CanSeePlayer)
                m_EnemyReadyToAttackSO.RaiseEvent(_agent.transform.position);
            else
                ResetHasChargedFlag();   
        }

        private void BackOff(Vector2 chargeDirection)
        {
            if (_enemyAwarenessController.CanSeePlayer)
            {
                // Add a force to the rigidbody in the opposite direction
                _rb2D.AddForce(-chargeDirection * 50f);

                // Reset the timer
                _chargeTimer.ResetTimer();

                // Start the timer
                _chargeTimer.StartTheTimer(chargeDirection);
            }
            else
            {
                ResetHasChargedFlag();
            }    

        }

        // A method that makes the enemy charge towards the player head
        public void ChargeToHead(Vector2 chargeDirection)
        {
            if (_enemyAwarenessController.CanSeePlayer)
            {
                // Add a force to the rigidbody in the same direction (towards player's head)
                _rb2D.AddForce(chargeDirection * 30f, ForceMode2D.Impulse);
                
                // Reset the timer
                _chargeTimer.ResetTimer();
                
                // Start the timer that is going to reset the hasCharged flag, which will result to recharging to player
                // to be made available again
                _chargeTimer.StartSecondTimer();
            }
            else
            {
                ResetHasChargedFlag();
            }
        }
       public void ResetHasChargedFlag()
        {
            _hasCharged = false;
        }
    }
}
