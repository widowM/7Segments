using UnityEngine;
using Cinemachine;
using WormGame.FactoryPattern;
using WormGame.EventChannels;
using WormGame.Core.Utils;
using System;
using System.Net.WebSockets;
using System.Collections.Generic;

namespace WormGame.Core.Player
{
    /// <summary>
    /// /// The WormBuilder is responsible for the initialization of the worm using the WormDataSO.
    /// It uses the Awake method to clear the collections, initialize the worm data and physics,
    /// and build the worm head and body. It also updates the active segments number text UI and
    /// sets the Cinemachine camera to follow the worm head.
    /// </summary>
    public class WormBuilder : MonoBehaviour
    {
        [SerializeField] private Worm _worm;
        [SerializeField] private WormSegmentConcreteFactory _wormSegFactory;
        [SerializeField] private Transform _wormSpawnPoint;
        [SerializeField] private ObjectPooler _objPooler;
        [Header("Listen to Event Channels")]
        [SerializeField] private VoidEventChannelSO _startCreatingLevelSO;
        [Header("Broadcast on Event Channels")]
        [SerializeField] IntEventChannelSO _updatingActiveSegmentsNumberTextUISO;
        [SerializeField] private VoidEventChannelSO _headReferencedSO;
        [SerializeField] private VoidEventChannelSO _wormReadySO;

        private void Initialize()
        {
            _worm.WormCollectionsDataSO.ClearWormSegmentsCollection();
            _worm.WormCollectionsDataSO.ClearWormChainsCollections();
            _worm.WormCollectionsDataSO.ClearPowerUpsSOsCollection();

            WormDataSO wormSO = _worm.WormDataSO;
            WormPhysicsDataSO wormPhysicsSO = _worm.WormPhysicsDataSO;

            wormSO.InitializeWorm();
            wormPhysicsSO.InitializeWormSpeed();

            BuildWormHead();
            _objPooler.Initialize();
            BuildWormBody();
        }


        private void BuildWormHead()
        {
            // Create the head segment of the worm from the Worm Data scriptable object
            // The head segment has a different role and layer than the other segments 
            // This allows for collision detection with the environment and the enemies
            GameObject headSegment = Instantiate(_worm.WormDataSO.WormHeadPrefab,
                _wormSpawnPoint.position, Quaternion.identity);

            Rigidbody2D headRigidbody = headSegment.GetComponent<Rigidbody2D>();
            List<GameObjectRB2D> connectedSegments = _worm.WormCollectionsDataSO.ConnectedSegments;

            connectedSegments.Add(new GameObjectRB2D(headSegment, headRigidbody));

            // Store a reference to the head segment in the Worm Data scriptable object 
            // This allows the enemies to target the head without using a Singleton pattern
            _worm.WormDataSO.Head = headSegment;
            _headReferencedSO.RaiseEvent();
        }

        private void BuildWormBody()
        {
            int initialWormSegmentsNum = _worm.WormStructureDataSO.InitialNumberOfWormSegments;
            //Vector2 headPosition = _worm.WormDataSO.Head.transform.position;
            float segmentOffset = _worm.WormStructureDataSO.SegmentOffsetXPos;
            List<GameObjectRB2D> connectedSegments = _worm.WormCollectionsDataSO.ConnectedSegments;

            for (int i = 1; i < initialWormSegmentsNum; i++)
            {
                // Calculate the x position of the next segment based on the current segment index
                // This ensures that current segment sits in the right position and the segments
                // are aligned.
                Vector3 segmentPosition = (Vector3)_wormSpawnPoint.position + Vector3.right * i * segmentOffset;

                GameObject wormSegment = WormUtils.CreateSegmentToConnectOnWorm(segmentPosition, _wormSegFactory);

                WormUtils.AddConnectedSegmentOnWormCollection(wormSegment, connectedSegments, i);
                wormSegment.transform.rotation = connectedSegments[i - 1].Go.transform.rotation;
                // Get the hinge joint component of the current segment
                HingeJoint2D hinge = wormSegment.GetComponent<HingeJoint2D>();

                // Connect the hinge joint to the previous segment’s rigidbody
                hinge.connectedBody = connectedSegments[i - 1].Rb2D;
            }

            _wormReadySO.RaiseEvent();
        }

        private void OnEnable()
        {
            _startCreatingLevelSO.OnEventRaised += Initialize;
        }

        private void OnDisable()
        {
            _startCreatingLevelSO.OnEventRaised -= Initialize;
        }
    }
}