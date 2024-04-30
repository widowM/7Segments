using UnityEngine;
using WormGame.Core;

namespace WormGame
{
    public class BombDamageLogic : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IDamageable damageableObject) &&
                collision.gameObject.CompareTag("Enemy"))
            {
                Vector2 hitPos = collision.transform.position;
                Vector2 knockbackDir = collision.transform.position - transform.position;
                DamageInfo damageInfo = new DamageInfo(hitPos, knockbackDir * 4);
                damageableObject.TakeDamage(damageInfo);
            }
        }
    }
}
