using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using WormGame.Core;
using WormGame.EventChannels;

namespace WormGame.Core.Player
{
    /// <summary>
    /// This class is responsible for adjusting the physics properties of the worm segments based on the worm length.
    /// It uses event channels to listen to the changes in the worm state and tweak the linear and angular drag,
    /// and the mass of each segment. It also uses the worm data and the worm physics scriptable object to access
    /// the relevant parameters and values.
    /// </summary>
    public class WormPhysics : MonoBehaviour
    {
        [SerializeField] private Worm _worm;

        [Header("Listen to Event Channels")]
        [SerializeField] private VoidEventChannelSO _tweakingWormPhysicsPropertiesSO;


        // TODO: Not actually needed
        private void TweakWormPhysics()
        {
            WormPhysicsDataSO wormPhysicsData = _worm.WormPhysicsDataSO;

            int currentWormLength = _worm.WormDataSO.CurrentWormLength;
            float totalMass = 8;
            float drag = wormPhysicsData.TotalLinearDrag / currentWormLength;
            float angularDrag = wormPhysicsData.TotalAngularDrag / currentWormLength;
            float mass = totalMass / _worm.WormDataSO.CurrentWormLength;
            List<GameObjectRB2D> connectedSegments = _worm.WormCollectionsDataSO.ConnectedSegments;

            for (int i = 0; i < connectedSegments.Count; i++)
            {
                Rigidbody2D rb = connectedSegments[i].Rb2D;
                rb.drag = drag;
                rb.angularDrag = angularDrag;
                rb.mass = mass;
            }
        }

        private void OnEnable()
        {
            _tweakingWormPhysicsPropertiesSO.OnEventRaised += TweakWormPhysics;
        }
        private void OnDisable()
        {
            _tweakingWormPhysicsPropertiesSO.OnEventRaised -= TweakWormPhysics;
        }
    }
}