using System.Collections.Generic;
using UnityEngine;
using WormGame.FactoryPattern;
using WormGame.EventChannels;
using WormGame.Core.Utils;
using WormGame.Core.Containers;

namespace WormGame.Core.Player
{


    public class WormMechanicHelper : MonoBehaviour
    {
        [SerializeField] private WormDataSO _wormDataSO;
        [SerializeField] private WormCollectionsDataSO _wormCollectionsDataSO;
        [SerializeField] private WormStructureDataSO _wormStructureDataSO;
        [SerializeField] private ObjectPoolTagsContainerSO _objectPoolTagsContainerSO;
        [SerializeField] private WormSegmentConcreteFactory _segFactory;
        [SerializeField] private WormDisconnectedSegmentConcreteFactory _disconnectedSegFactory;
        private float _hingeAnchor;
        [Header("Broadcast on Event Channels")]
        [Tooltip("Notifies listeners that a worm segment has been connected to the main body")]
        [SerializeField] private VoidEventChannelSO _wormSegmentConnectingSO;
        [Tooltip("Notifies listeners that disconnection of body parts has happened")]
        [SerializeField] private VoidEventChannelSO _wormSegmentDisconnectingSO;
        [Tooltip("Notifies listeners that active number of segments text of UI should be updated")]
        [SerializeField] private IntEventChannelSO _updatingActiveSegmentsNumberTextUISO;
        [Tooltip("Notifies listeners that physics of each segment is going to be recalculated to match new length of worm")]
        [SerializeField] private VoidEventChannelSO _tweakingWormPhysicsDataSO;
        [Header("Listen to Event Channels")]
        [SerializeField] private VoidEventChannelSO _wormReadySO;

        public WormDataSO WormDataSO => _wormDataSO;
        public WormStructureDataSO WormStructureDataSO => _wormStructureDataSO;
        public WormCollectionsDataSO WormCollectionsDataSO => _wormCollectionsDataSO;
        public ObjectPoolTagsContainerSO ObjectPoolTagsContainerSO => _objectPoolTagsContainerSO;
        public WormSegmentConcreteFactory SegmentFactory => _segFactory;
        public float Anchor => _hingeAnchor;
        public VoidEventChannelSO WormSegmentConnectingSO => _wormSegmentConnectingSO;
        public VoidEventChannelSO WormSegmentDisconnectingSO => _wormSegmentDisconnectingSO;     

        
        // Public methods
        public void OnWormLengthValueChangedWrapper(int length)
        {
            OnWormLengthValueChanged(length);
        }

        public void OnAttachedWrapper()
        {
            OnAttached();
        }

        public void ConnectChainToWormWrapper(in int finalWormLength, in float anchor)
        {
            ConnectChainToWorm(finalWormLength, anchor);
        }

        // Private methods
        private void OnEnable()
        {
            _wormReadySO.OnEventRaised += Initialize;
        }

        private void OnDisable()
        {
            _wormReadySO.OnEventRaised -= Initialize;
        }
        private void Initialize()
        {
            _hingeAnchor = GetAnchor();
            //OnTweakingWormPhysics();
        }

        private void OnTweakingWormPhysics()
        {
            _tweakingWormPhysicsDataSO.RaiseEvent();
        }
        private float GetAnchor()
        {
            return _wormStructureDataSO.SegmentSpacing / 2;
        }

        private void OnWormLengthValueChanged(int length)
        {
            _wormDataSO.SetCurrentWormLength(length);
            if (_updatingActiveSegmentsNumberTextUISO != null)
                _updatingActiveSegmentsNumberTextUISO.RaiseEvent(_wormDataSO.CurrentWormLength);
        }

        private void OnAttached()
        {
            _wormSegmentConnectingSO.RaiseEvent();
        }

        public void UpdateWormAppearance(int change)
        {
            List<GameObjectRB2D> connectedSegments = _wormCollectionsDataSO.ConnectedSegments;
            int totalLength = _wormDataSO.CurrentWormLength + change;

            for (int i = 1; i < connectedSegments.Count; i++)
            {
                (Vector2[], Quaternion) posRot = CalculateNewSegmentPositionAndRotation(connectedSegments, i);

                string poolTag = connectedSegments[i].Go.GetComponent<PoolNameHolder>().PoolName.Value;
                ObjectPooler.Instance.ReturnObject(connectedSegments[i].Go, poolTag);
                GameObject seg = GetSuitableSegment(i, totalLength, posRot);
                AddSegGORbPairToConnectedSegsCollection(seg, connectedSegments, i);

                // Connect the segments
                ConnectConsecutiveSegments(connectedSegments, i);
            }
        }

        private (Vector2[], Quaternion) CalculateNewSegmentPositionAndRotation(List<GameObjectRB2D> connectedSegments, int connectedSegsChainIndex)
        {
            Vector2[] pos = { connectedSegments[connectedSegsChainIndex].Go.transform.position };
            Quaternion rot = connectedSegments[connectedSegsChainIndex].Go.transform.rotation;

            return (pos, rot);
        }

        private GameObject GetSuitableSegment(int connectedSegsChainIndex, int totalWormVisualLength, (Vector2[], Quaternion) posRot)
        {
            GameObject seg;
            if (connectedSegsChainIndex < totalWormVisualLength)
            {
                // Spawn in the right position
                seg = _segFactory.GetProduct<IProduct>(posRot.Item1).GameObject;
            }
            else
            {
                // Spawn in the right position
                seg = _disconnectedSegFactory.GetProduct<IProduct>(posRot.Item1).GameObject;
            }
            seg.transform.rotation = posRot.Item2;

            return seg;
        }

        private void AddSegGORbPairToConnectedSegsCollection(GameObject seg, List<GameObjectRB2D> connectedSegments, int connectedSegsChainIndex)
        {
            Rigidbody2D segRb = seg.GetComponent<Rigidbody2D>();
            GameObjectRB2D newGORBPair = new GameObjectRB2D(seg, segRb);
            // Add it to the collection of visible segments
            connectedSegments[connectedSegsChainIndex] = newGORBPair;
        }
        private void ConnectConsecutiveSegments(List<GameObjectRB2D> connectedSegments, int connectedSegsChainIndex)
        {
            HingeJoint2D hJ = connectedSegments[connectedSegsChainIndex].Go.GetComponent<HingeJoint2D>();

            hJ.connectedBody = connectedSegments[connectedSegsChainIndex - 1].Rb2D;
        }
        public int CalculateWormLengthAfterAttachment(in int lengthOfChainOfSegments)
        {
            int connectedSegmentsCount = _wormDataSO.CurrentWormLength;

            return lengthOfChainOfSegments + connectedSegmentsCount;
        }

        private void ConnectChainToWorm(in int finalWormLength, in float anchor)
        {
            int currentSegmentsCount = _wormCollectionsDataSO.ConnectedSegments.Count;

            const float radius = 1.8f;

            for (int i = currentSegmentsCount; i < finalWormLength; i++)
            {
                Vector2 validSpawnPos = GetValidSpawnPosition(i, radius);
                GameObject wormSegment = WormUtils.CreateSegmentToConnectOnWorm(validSpawnPos, _segFactory);
                WormUtils.AddConnectedSegmentOnWormCollection(wormSegment, _wormCollectionsDataSO.ConnectedSegments, i);
                SetRotationEqualToHead(wormSegment);
                ConnectHingeJoints(wormSegment, i, anchor);
            }
        }

        public Vector2 GetValidSpawnPosition(int index, float radius)
        {
            Vector2 previousSegmentPosition = CalculatePreviousSegmentPosition(index);
            Vector2 wormSegmentSpawnPosition = CalculateNextSegmentPosition(previousSegmentPosition);

            Vector2 validSpawnPos =
                SpawnUtils.ConvertToValidWormSegmentSpawnPosition(wormSegmentSpawnPosition,
                previousSegmentPosition, radius);

            return validSpawnPos;
        }

        private void SetRotationEqualToHead(GameObject wormSegment)
        {
            Quaternion headRotation = _wormDataSO.Head.transform.rotation;
            wormSegment.transform.rotation = headRotation;
        }
        private void ConnectHingeJoints(GameObject wormSegment, int currentIndex, float anchor)
        {
            List<GameObjectRB2D> connectedSegments = _wormCollectionsDataSO.ConnectedSegments;
            HingeJoint2D hinge = wormSegment.GetComponent<HingeJoint2D>();
            hinge.connectedBody = connectedSegments[currentIndex - 1].Rb2D;
        }

        public Vector2 CalculateNextSegmentPosition(Vector2 previousSegmentPosition)
        {
            float offsetX = _wormStructureDataSO.SegmentOffsetXPos;
            Vector2 offset = previousSegmentPosition * Vector2.right * offsetX;

            return previousSegmentPosition - offset;
        }

        public Vector2 CalculatePreviousSegmentPosition(int index)
        {
            List<GameObjectRB2D> connectedSegments = _wormCollectionsDataSO.ConnectedSegments;
            Vector2 previousSegmentPosition = connectedSegments[index - 1].Go.transform.position;

            return previousSegmentPosition;
        }
    }
}