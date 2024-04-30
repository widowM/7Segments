using System.Collections.Generic;
using UnityEngine;
using WormGame.Core.Containers;
using WormGame.Core.Utils;
using WormGame.FactoryPattern;
using WormGame.Managers;

namespace WormGame.Core.Player
{
    // Not needed for the current iteration of the game
    /// <summary> 
    /// This class is responsible for generating worm chains during runtime. It uses the WormUtils static
    /// class as a Utility class.
    /// </summary>
    public class WormChainGenerator : MonoBehaviour
    {
        [SerializeField] private WormDataSO _wormDataSO;
        [SerializeField] private WormCollectionsDataSO _wormCollectionsDataSO;
        [SerializeField] private WormStructureDataSO _wormStructureDataSO;

        [SerializeField] private WormConnectableSegmentConcreteFactory _connectableSegFactory;
        [SerializeField] private WormDisconnectedSegmentConcreteFactory _disconnectedSegFactory;
        [SerializeField] private ObjectPoolTagsContainerSO _poolTagsContainerSO;
        [SerializeField] private CompositeCollider2D _spawnableAreaCol;

        [Header("Listen to Event Channels")]
        [SerializeField] private IntVector2EventChannelSO _generatingDetachedChainSO;


        private void GenerateChainOfRandomLengthAtRandomPosition()
        {
            Vector2 spawnPosition = SpawnUtils.GetValidSpawnPositionOnMap(in _spawnableAreaCol);
           
            // Constrained the generated chains at a length between 1 and 3 for testing
            int randomLength = Random.Range(1, 4);

            List<GameObject> disconnectedChain = WormUtils.CreateDisconnectedChain();

            GameObject connectableSegment =
                WormUtils.CreateSegmentAtPosition(_connectableSegFactory, spawnPosition);
            if (_wormDataSO.WormCollectionsDataSO.WormDisconnectedChainsOfSegments.ContainsKey(connectableSegment))
            {
                Debug.Log("The key was already inside the dictionary");
                ObjectPooler.Instance.ReturnObject(connectableSegment, _poolTagsContainerSO.WormSegmentConnectableTag);

                return;
            }
            WormUtils.AddNewEntryToDictionary(connectableSegment, disconnectedChain, _wormDataSO);
            CompleteRandomChainGeneration(_disconnectedSegFactory, randomLength, disconnectedChain);
        }

        private void CompleteRandomChainGeneration(in Factory segmentFactory, in int length, List<GameObject> disconnectedChain)
        {
            // The radius of the circle that is tangent to the previous segment
            const float tangentCircleRadius = 1.6f;

            for (int i = 1; i < length; i++)
            {
                // Find a valid spawn position on worm that does not overlap with other objects
                Vector2 validSpawnPos = SpawnUtils.
                    GetValidWormSegmentSpawnPositionForRandomChain(disconnectedChain, in i, tangentCircleRadius);

                WormUtils.AttachSegment(segmentFactory, validSpawnPos, disconnectedChain, _wormDataSO);
            }
        }

        private void BeginGeneratingChainOfLengthAtPosition(int length, Vector2 initialPosition)
        {
            WormUtils.
                GenerateChainOfLengthAtDetachmentPosition
                (length,
                initialPosition,
                _connectableSegFactory,
                _disconnectedSegFactory,
                _wormDataSO);
        }

        private void OnEnable()
        {
            _generatingDetachedChainSO.OnEventRaised += BeginGeneratingChainOfLengthAtPosition;
        }

        private void OnDisable()
        {
            _generatingDetachedChainSO.OnEventRaised -= BeginGeneratingChainOfLengthAtPosition;
        }
    }
}