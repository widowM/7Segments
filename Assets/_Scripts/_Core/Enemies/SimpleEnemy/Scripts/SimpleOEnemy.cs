using System;
using UnityEngine.AI;

namespace WormGame.Core.Enemies
{
    /// <summary>
    /// This class implements the IDamageable interface and handles the enemy damage logic.
    /// </summary>
    /// <remarks>
    /// This class calls methods from other classes to manage the enemy health, visual, and physics
    /// effects when taking damage. It acts as a connector between the EnemyHealth, EnemyVisual, and EnemyPhysics classes.
    /// </remarks>

    public class SimpleOEnemy : Enemy
    {
        // Methods
        void Start()
        {
            _collider2D.isTrigger = false;

            // Setting a random steering speed for the SimpleO Enemy
            GetComponent<NavMeshAgent>().speed = UnityEngine.Random.Range(1.2f, 1.6f);
        }
    }
}