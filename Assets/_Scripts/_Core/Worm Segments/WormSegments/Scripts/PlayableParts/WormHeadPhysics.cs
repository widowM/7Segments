using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WormGame.Core
{
    public class WormHeadPhysics : MonoBehaviour
    {
        [SerializeField] private WormHead _wormHead;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IInteractable IInteractableObject))
                IInteractableObject.Interact(gameObject);

            if (collision.gameObject.TryGetComponent(out IDamageable IDamageableObject))
            {
                // TODO: Get rid of this flag with IWormState
                if (_wormHead.WormDataSO.IsPowerUpd)
                {
                    DamageInfo damageInfo = new DamageInfo();
                    IDamageableObject.TakeDamage(damageInfo);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IInteractable IInteractableObject))
            {
                IInteractableObject.Interact(gameObject);
            }
        }
    }
}