using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WormGame.Core
{
    // TODO: Add different types of projectiles
    /// <summary>
    /// This class inherits from ScriptableObject and defines the data for the worm’s projectile attack.
    /// It has the following fields and properties: // - Speed: a float that represents the initial velocity
    /// of the projectile, in meters per second. // - AccelerationFactor: a float that represents the rate of increase
    /// of the projectile’s speed over time, in meters per second squared. // - MaxDistance: a float that represents
    /// the maximum distance that the projectile can travel before it is destroyed, in meters. // The fields are serialized
    /// and can be adjusted in the inspector. // The class can be used to create different types of projectiles for the worm.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/WormGame/Worm/WormProjectileDataSO")]
    public class WormProjectileDataSO : ScriptableObject
    {
        [SerializeField] private float _speed = 25;
        [SerializeField] private float _maxDistance = 10;
        public float _accelerationFactor = 0.1f;

        public float Speed => _speed;
        public float AccelerationFactor => _accelerationFactor;
        public float MaxDistance => _maxDistance;
    }
}