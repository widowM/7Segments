using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Core;

namespace WormGame
{
    public class ParticleCollisionHandler : MonoBehaviour
    {
        public ParticleSystem part;
        private List<ParticleCollisionEvent> _collisionEvents;

        void Start ()
        {
            _collisionEvents = new List<ParticleCollisionEvent>();
        }
        private void OnParticleCollision(GameObject other)
        {
            if (other.TryGetComponent(out IDamageable damageableObject) &&
            other.CompareTag("Enemy"))
            {
                //Debug.Log("Particle collided with: " + other.name);
                int numCollisionEvents = part.GetCollisionEvents(other, _collisionEvents);

                Rigidbody2D rb2D = other.GetComponent<Rigidbody2D>();
                if (rb2D)
                {
                    int i = 0;

                    while (i < numCollisionEvents)
                    {
                        Vector3 pos = _collisionEvents[i].intersection;
                        DamageInfo damageInfo = new DamageInfo(pos, rb2D.velocity * 2);
                        damageableObject.TakeDamage(damageInfo);
                        i++;
                    }
                }         
            }                    
        }
    }
}
