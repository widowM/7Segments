using UnityEngine;
using WormGame.Core;

/// <summary>
/// Tooth weapon behaviour
/// </summary>
public class WormJaws : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb2D;
    [SerializeField] private Collider2D _headCollider;
    [SerializeField] private WormDataSO _wormDataSO;

    private void Update()
    {
        transform.rotation = _wormDataSO.Head.transform.rotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable damageableObject) &&
            collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 hitPos = collision.transform.position;
            DamageInfo damageInfo = new DamageInfo(hitPos, _rb2D.velocity * 2);
            damageableObject.TakeDamage(damageInfo);
        }
    }
}