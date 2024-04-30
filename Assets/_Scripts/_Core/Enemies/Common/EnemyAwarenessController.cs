using UnityEngine;
using WormGame.Core.Utils;
using WormGame.EventChannels;

namespace WormGame.Core.Enemies
{
    public class EnemyAwarenessController : MonoBehaviour
    {
        private bool _canSeePlayer;
        
        [SerializeField] private LayerMask _obstacleLayer;
        private RaycastHit2D[] _results = new RaycastHit2D[10];
        [Header("Broadcast on Event Channels")]
        [SerializeField] private Vector2EventChannelSO _playEnemySoundAtPointSO;

        public bool CanSeePlayer
        {
            get { return _canSeePlayer; }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Head"))
            {
                Vector2 dirToPlayer = WormUtils.GetDirectionToTarget(collision.gameObject.transform.position, transform.position);
                bool obstacleBetweenExists = ObstacleDetectedBetweenIandPlayer(dirToPlayer);
                _canSeePlayer = !obstacleBetweenExists;
                _playEnemySoundAtPointSO?.RaiseEvent(transform.position);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Head"))
            {
                _canSeePlayer = false;

            }
        }

        private bool ObstacleDetectedBetweenIandPlayer(Vector2 dirToPlayer)
        {
            int hitCount = Physics2D.RaycastNonAlloc(transform.position, dirToPlayer, _results, dirToPlayer.magnitude, _obstacleLayer);

            for (int i = 0; i < hitCount; i++)
            {
                // Check if the collider is not the player, hence an obstacle
                if (_results[i].collider != null)
                {
                    //Debug.Log("Obstacle detected: " + _results[i].collider.gameObject.name);
                    return true;
                }
            }

            return false;
        }
    }
}
