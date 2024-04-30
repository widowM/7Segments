using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WormGame.Core.Enemies
{
    /// <summary>
    /// This class handles the physics behavior of the enemy.
    /// </summary>
    /// <remarks>
    /// This class uses the Rigidbody2D component to apply forces
    /// and impulses to the enemy. It also has a method that knocks
    /// the enemy back when it takes damage, based on the velocity of the
    /// projectile.
    /// </remarks>
    /// 
    public class EnemyPhysics : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private Rigidbody2D _rb2D;
        [SerializeField] private Collider2D _collider;

        public void Knockback(Vector2 projectileVelocity)
        {
            _rb2D.AddForce(projectileVelocity * _enemy.EnemySO.KnockbackAmount, ForceMode2D.Impulse);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IDamageable IDamageableObject) && 
                !collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("Weapon"))
            {
                //Debug.Log("Collided with: " + collision.gameObject.name);
                AudioSource
                    .PlayClipAtPoint
                    (_enemy.EnemySO.ImpactSfx, collision.contacts[0].point,
                    _enemy.EnemySO.ImpactSfxVolume);
                DamageInfo damageInfo = new DamageInfo(collision.GetContact(0).point, _rb2D.velocity);
                IDamageableObject.TakeDamage(damageInfo);
            }         
        }

        public void DisableCollider()
        {
            _collider.enabled = false;
        }
    }
}