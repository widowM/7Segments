using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WormGame.FactoryPattern;

namespace WormGame.Core.Utils
{
    // Utility class for the worm chain mechanics
    public static class WormUtils
    {
        public static WormDataSO wormDataSO;

        public static GameObject CreateSegmentToConnectOnWorm(Vector2 spawnPos, Factory m_Factory)
        {
            Vector2[] spawnPosArg = { spawnPos };
            IProduct wormSegmentProduct = m_Factory.GetProduct<IProduct>(spawnPosArg);
            GameObject wormSegment = wormSegmentProduct.GameObject;

            return wormSegment;
        }

        static WormUtils()
        {
            Debug.Log("Asset database requested.");
#if UNITY_EDITOR
            string[] guids =
                AssetDatabase.FindAssets("t:WormDataSO", new[] { "Assets/Resources/" });
            // Load the first one using its GUID
            wormDataSO = AssetDatabase.LoadAssetAtPath<WormDataSO>(AssetDatabase.GUIDToAssetPath(guids[0]));
#endif
#if UNITY_STANDALONE
            // Use Resources.Load in the build
            // Make sure the scriptable object is in the Resources folder
            wormDataSO = Resources.Load<WormDataSO>("WormDataSO");
#endif
        }
        public static void AddConnectedSegmentOnWormCollection(GameObject wormSegment, List<GameObjectRB2D> connectedSegments, int indexInList)
        {
            Rigidbody2D wormSegmentRigidbody = wormSegment.GetComponent<Rigidbody2D>();

            connectedSegments.Add(new GameObjectRB2D(wormSegment, wormSegmentRigidbody));

            // Store the value of the worm segment index in collection of connected segments
            // needed for the disconnection of segments
            wormSegment.GetComponent<WormSegmentDefault>().IndexInList = indexInList;
        }

        public static void GenerateChainOfLengthAtDetachmentPosition
            (int length, Vector2 initialPosition, Factory wormConnectableFac, Factory wormDisconnectedFac, WormDataSO wormSO)
        {
            List<GameObject> disconnectedChain = CreateDisconnectedChain();

            GameObject connectableSegment =
                CreateSegmentAtPosition(wormConnectableFac, initialPosition);

            AddNewEntryToDictionary(connectableSegment, disconnectedChain, wormSO);
            CompleteDetachedChainGeneration(wormDisconnectedFac, length, disconnectedChain, wormSO);
        }

        public static List<GameObject> CreateDisconnectedChain()
        {
            return new List<GameObject>();
        }

        public static GameObject CreateSegmentAtPosition(in Factory segmentFactory, Vector2 validPosition)
        {
            Vector2[] validPositionArg = { validPosition };
            IProduct segmentProduct = segmentFactory.GetProduct<IProduct>(validPositionArg);
            GameObject segment = segmentProduct.GameObject;
            segment.transform.position = validPosition;

            return segment;
        }

        public static void AddNewEntryToDictionary(in GameObject connectableKey, List<GameObject> disconnectedChain, WormDataSO wormSO)
        {
            disconnectedChain.Add(connectableKey);
            wormSO.WormCollectionsDataSO.WormDisconnectedChainsOfSegments.Add(connectableKey, disconnectedChain);
        }

        public static void CompleteDetachedChainGeneration(in Factory segmentFactory, in int length, List<GameObject> disconnectedChain, WormDataSO wormSO)
        {
            // The radius of the circle that is tangent to the previous segment
            float tangentCircleRadius = wormSO.WormStructureDataSO.SegmentOffsetXPos;
            for (int i = 1; i < length; i++)
            {
                // Find a valid spawn position on worm that does not overlap with other objects
                Vector2 validSpawnPos = SpawnUtils.
                    GetValidWormSegmentSpawnPositionForDetachedChain(disconnectedChain, i, length, tangentCircleRadius, wormSO.WormCollectionsDataSO);

                AttachSegment(segmentFactory, validSpawnPos, disconnectedChain, wormSO);
            }
        }

        public static void AttachSegment(in Factory segmentFactory, Vector2 validSpawnPos, List<GameObject> disconnectedChain, WormDataSO wormSO)
        {
            // Create a segment at the valid spawn position
            GameObject disconnectedSegment = CreateSegmentAtPosition(segmentFactory, validSpawnPos);

            // Add the segment to the chain
            disconnectedChain.Add(disconnectedSegment);

            // Connect the segment to the previous ->segment<-
            ConnectSegmentToPrevious(disconnectedChain, disconnectedSegment, wormSO);
        }

        public static void ConnectSegmentToPrevious(List<GameObject> disconnectedChain, in GameObject wormDisconnectedSegment, WormDataSO wormSO)
        {
            HingeJoint2D hinge = wormDisconnectedSegment.GetComponent<HingeJoint2D>();

            int chainCurrentLength = disconnectedChain.Count;
            int chainListCurrentIndex = chainCurrentLength - 1;

            Rigidbody2D chainPreviousRigidbody2D = disconnectedChain[chainListCurrentIndex - 1].
                GetComponent<Rigidbody2D>();

            
            hinge.connectedBody = chainPreviousRigidbody2D;
        }

        public static void GenerateSavedChainOfLengthAtPosition(int chainLength, List<Point> savedSpawnPosition)
        {
            List<GameObject> disconnectedChain = CreateDisconnectedChain();
            Factory m_WormConnectableFactory = GameObject.FindGameObjectWithTag("WormConnectableFactory").GetComponent<Factory>();
            Factory m_WormDisconnectedFactory = GameObject.FindGameObjectWithTag("WormDisconnectedFactory").GetComponent<Factory>();
            Vector2 connectablePos = new Vector2(savedSpawnPosition[0].x, savedSpawnPosition[0].y);
            GameObject connectableSegment = CreateSegmentAtPosition(m_WormConnectableFactory, connectablePos);

            AddNewEntryToDictionary(connectableSegment, disconnectedChain, wormDataSO);
            CompleteDetachedChainGeneration(m_WormDisconnectedFactory, chainLength, disconnectedChain, wormDataSO);

            for (int i = 0; i < disconnectedChain.Count; i++)
            {
                Vector2 segmentPos = new Vector2(savedSpawnPosition[i].x, savedSpawnPosition[i].y);
                disconnectedChain[i].transform.position = segmentPos;
            }
        }

        public static Vector2 GetDirectionToTarget(Vector2 targetPos, Vector2 myPos)
        {
            return (targetPos - myPos);
        }
    }
}