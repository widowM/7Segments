using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.EventChannels;

namespace WormGame.Core
{
    /// <summary>
    /// Worm physics data for gameplay fine-tuning
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/WormGame/Worm/WormPhysicsData", fileName = "WormPhysicsData")]
    public class WormPhysicsDataSO : ScriptableObject
    {
        [Header("Worm Physics Properties")]
        [Tooltip("The worm's movement speed")]
        [Range(0, 30)]
        [SerializeField] private float _wormMovementSpeed = 10;

        [Tooltip("The worm's head rotation speed")]
        [Range(0.1f, 15)]
        [SerializeField] private float _wormHeadRotationSpeed = 1;

        [Range(25f, 100)]
        [Tooltip("(Rigidbody2D) The total linear drag of the worm's combined rigidbodies.")]
        [SerializeField] private float _totalLinearDrag;

        [Range(0f, 50)]
        [Tooltip("(Rigidbody2D) The total angular drag of the worm's combined rigidbodies.")]
        [SerializeField] private float _totalAngularDrag;

        [Tooltip("The shooting force of the worm boomerang segments.")]
        [SerializeField] private float _shootingForce = 106;

        [Tooltip("Press Tweak Worm Physics editor button to set new worm physics values.")]
        [SerializeField] private VoidEventChannelSO _tweakingWormPhysics;


        public float WormMovementSpeed
        {
            get { return _wormMovementSpeed; }
            set { _wormMovementSpeed = Mathf.Clamp(value, 0.1f, 60); }
        }

        public float WormHeadRotationSpeed => _wormHeadRotationSpeed;
        public float ShootingForce => _shootingForce;
        public float TotalLinearDrag
        {
            get { return _totalLinearDrag; }
            set { _totalLinearDrag = Mathf.Clamp(value, 25f, 100); }
        }
        public float TotalAngularDrag
        {
            get { return _totalAngularDrag; }
            set { _totalAngularDrag = Mathf.Clamp(value, 0f, 50); }
        }


        public VoidEventChannelSO TweakingWormPhysics => _tweakingWormPhysics;

        public void InitializeWormSpeed()
        {
            _wormMovementSpeed = 10f;
        }
    }
}