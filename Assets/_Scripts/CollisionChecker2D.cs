using System;
using UnityEngine;
using WormGame.Core.Utils;

/// <summary>
/// Class to detect collision during edit mode, e.g., during Level Editing
/// </summary>
[ExecuteInEditMode]
public class CollisionChecker2D : MonoBehaviour
{
    public float radius = 0.5f; // the radius of the circle to check for collisions

    private Collider2D[] _spawnAreaColliders;
    private Collider2D[] _otherColliders;
    private int maxColliders = 10;
    public Color gizmoColor = Color.red;
    public Color greenColor = Color.green;// the color of the circle gizmo

    private bool _canSpawn = false;
    private Collider2D _polygonCollider;

    public bool CanSpawn => _canSpawn;

    private void OnEnable()
    {

        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer r in srs)
        {
            r.sortingLayerName = "AboveAll";
        }

        _polygonCollider = GetComponentInChildren<Collider2D>();
        _polygonCollider.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        _spawnAreaColliders = new Collider2D[maxColliders];
        _otherColliders = new Collider2D[maxColliders];
    }

    void Update()
    {
        // Get the bounds of the polygon collider
        
        Bounds polygonBounds = _polygonCollider.bounds;

        // get the colliders that overlap with the circle
        // Find all colliders on the spawn area layer
        Array.Clear(_spawnAreaColliders, 0, _spawnAreaColliders.Length);
        Array.Clear(_otherColliders, 0, _otherColliders.Length);

        int numSpawnAreaColliders = Physics2D.OverlapAreaNonAlloc(polygonBounds.min, polygonBounds.max, _spawnAreaColliders, SpawnUtils.LayersToSpawnOn);
        int numOtherColliders = Physics2D.OverlapAreaNonAlloc(polygonBounds.min, polygonBounds.max, _otherColliders, SpawnUtils.LayersNotToSpawnOn);

        if (numSpawnAreaColliders <= 0)
        {
            SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
            foreach (var sr in srs)
            {
                sr.color = gizmoColor;
            }
            _canSpawn = false;
        }

        else if (numOtherColliders > 0)
        {
            SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
            foreach (var sr in srs)
            {
                sr.color = gizmoColor;
            }
            _canSpawn = false;
        }

        // Else if it collides with spawnArea and not with any other object
        else
        {
            SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
            foreach (var sr in srs)
            {
                sr.color = greenColor;
            }
            _canSpawn = true;
        }
    }

    void OnDrawGizmos()
    {
        // draw a circle gizmo to visualize the collision area
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}