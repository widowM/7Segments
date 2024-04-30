using System.Collections;
using UnityEngine;
using WormGame.Core;
using WormGame.Variables;

public class LootDrop : MonoBehaviour
{
    [SerializeField] private StringVariableSO[] lootTags; // The loot prefab to drop
    public float forceMagnitude = 10f; // Initial force magnitude
    private int _dropCount;
    private WaitForSeconds _zeroThreeSeconds = new WaitForSeconds(0.3f);


    private void OnEnable()
    {
        StartCoroutine(SpawnLootCoroutine());
    }
    void SpawnLoot()
    {
        _dropCount = Random.Range(3, 5);

        for (int i = 0; i < _dropCount; i++)
        {
            StringVariableSO randomLootTag = lootTags[Random.Range(0, lootTags.Length)];
            Vector3 randomDirection = Random.insideUnitSphere;
            randomDirection.y = Mathf.Abs(randomDirection.y);
            GameObject loot = ObjectPooler.Instance.GetObject(randomLootTag.Value, transform.position, Quaternion.identity);
            Rigidbody2D rb2D = loot.GetComponent<Rigidbody2D>();
            rb2D.bodyType = RigidbodyType2D.Dynamic;
            rb2D.gravityScale = 1;
            rb2D.AddForce(randomDirection * forceMagnitude, ForceMode2D.Impulse);
            StartCoroutine(DisableGravityCoroutine(rb2D));
        }
    }

    IEnumerator SpawnLootCoroutine()
    {
        yield return _zeroThreeSeconds;
        SpawnLoot();
    }
    IEnumerator DisableGravityCoroutine(Rigidbody2D rb2D)
    {
        yield return _zeroThreeSeconds;
        rb2D.gravityScale = 0;
    }
}