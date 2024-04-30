using System.Collections;
using UnityEngine;
using WormGame.Core.Containers;
using WormGame.EventChannels;

namespace WormGame.Core.Player
{
    /// <summary> 
    /// This class is responsible for creating and controlling the worm projectile that can be shot by the player.
    /// It uses the worm data and the projectile data scriptable objects to access and alter the speed and direction
    /// of the projectile. It also uses collision detection and object pooling to handle the interactions of the
    /// projectile with other objects, such as enemies, the worm's head, and the bullet range. Also uses event channels
    /// to broadcast and listen to events related to the projectile.
    /// </summary>

    public class WormProjectile : MonoBehaviour
    {
        [SerializeField] private WormDataSO _wormDataSO;
        [SerializeField] private ObjectPoolTagsContainerSO _objectPoolTagsContainerSO;
        [SerializeField] private WormProjectileDataSO _projectileDataSO;
        [SerializeField] private Rigidbody2D _rb2D;

        // Declare a variable to store the initial force
        private float _initialReturningForce = 10f;
        private float _acceleration = 0.1f;
        private WaitForSeconds _waitForTwoSeconds = new WaitForSeconds(2f);

        // Flag to indicate if the projectile is returning
        private bool _returning = false;

        [Header("Broadcast on Event Channels")]
        [SerializeField] private VoidEventChannelSO _executeGainingOnePartSO;

        private void FixedUpdate()
        {
            if (_returning)
            {
                float currentForce = CalculateCurrentForce();
                Vector2 direction = CalculateProjectileDirectionRelativeToHead();
                _rb2D.AddForce(direction.normalized * _projectileDataSO.Speed * currentForce);
                _acceleration += Time.fixedDeltaTime;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_returning)
            {
                if (collision.gameObject.CompareTag("Head"))
                {
                    OnExecutingGainingOnePart();
                    ObjectPooler.Instance.
                        ReturnObject(gameObject, _objectPoolTagsContainerSO.WormProjectileTag);
                }
            }

            if (collision.gameObject.TryGetComponent(out IDamageable damageableObject) && collision.gameObject.CompareTag("Enemy"))
            {
                Vector2 hitPos = collision.ClosestPoint(transform.position);
                DamageInfo damageInfo = new DamageInfo(hitPos, _rb2D.velocity);
                damageableObject.TakeDamage(damageInfo);
                ObjectPooler.Instance.
                    ReturnObject(gameObject, _objectPoolTagsContainerSO.WormProjectileTag);
            }
        }

        private void OnExecutingGainingOnePart()
        {
            _executeGainingOnePartSO.RaiseEvent();
        }
        private float CalculateCurrentForce()
        {
            return _initialReturningForce * _acceleration;
        }

        private Vector2 CalculateProjectileDirectionRelativeToHead()
        {
            return _wormDataSO.Head.transform.position - transform.position;
        }
        private IEnumerator GoThenReturnToHeadCoroutine()
        {
            yield return _waitForTwoSeconds;
            _returning = true;
        }

        private void OnEnable()
        {
            StartCoroutine(GoThenReturnToHeadCoroutine());
        }

        private void OnDisable()
        {
            _returning = false;
            _acceleration = 0.1f;
        }
    }
}