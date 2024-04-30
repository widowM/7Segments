using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using WormGame.Managers;

namespace WormGame.Core.Enemies
{
    public class EnemyAI : MonoBehaviour, IUpdatable
    {
        [SerializeField] private NavMeshAgent _agent;

        // TODO: Replace this
        public void UpdateMe()
        {
            var turnTowardNavSteeringTarget = _agent.steeringTarget;
            Vector3 direction = (turnTowardNavSteeringTarget - transform.position).normalized;
            
            // Calculate the angle in radians
            float angle = Mathf.Atan2(direction.y, direction.x);

            // Convert the angle to degrees
            float angleDegrees = angle * Mathf.Rad2Deg;

            // Create a Quaternion rotation around the z-axis
            Quaternion lookRotation = Quaternion.Euler(0f, 0f, angleDegrees);

            float lerpSpeed = 3;
            float lerpFactor = lerpSpeed * Time.deltaTime;

            // Lerp the rotation towards the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, lerpFactor);
        }

        private void OnEnable()
        {
            UpdateManager.Instance.Register(this);
        }

        private void OnDisable()
        {
            UpdateManager.Instance.Unregister(this);
        }
    }
}