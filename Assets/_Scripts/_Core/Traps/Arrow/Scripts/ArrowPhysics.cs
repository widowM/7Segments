using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Core;
using WormGame.Core.Containers;

namespace WormGame.Core.Traps
{
    public class ArrowPhysics : MonoBehaviour
    {
        [SerializeField] private ObjectPoolTagsContainerSO _objectPoolTagsContainerSO;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            ObjectPooler.Instance.ReturnObject(gameObject, _objectPoolTagsContainerSO.RedArrowTag);

            if (collision.gameObject.TryGetComponent(out IDamageable damageableObject))
            {
                DoDamage(collision, damageableObject);
            }
        }

        private void DoDamage(Collision2D collision, IDamageable target)
        {
            Vector2 hitPos = collision.contacts[0].point;
            Vector2 projectileVel = GetComponent<Rigidbody2D>().velocity;
            DamageInfo damageInfo = new DamageInfo(hitPos, projectileVel);
            target.TakeDamage(damageInfo);
        }
    }
}