using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace WormGame.Core.Utils
{
    /// <summary>
    /// Utility class for custom spawning based on demands
    /// </summary>

    
    public static class SpawnUtils
    {
        private static readonly float _overlapCheckRadius = 1.9f; // A little shorter than the segment spacing
        private static readonly int _maxSpawnCheckAttempts = 200;
        private static LayerMask _layersNotToSpawnOn = LayerMask.GetMask(
            "WormSegment", "Head", "Environment", "DisconnectedSegment", "Enemy", "Arrow", "WormProjectile", "PowerUp"
            );
        private static LayerMask _layersToSpawnOn = LayerMask.GetMask("SpawnArea");

        private static Collider2D[] _collidersFound = new Collider2D[2];

        // Properties
        public static LayerMask LayersNotToSpawnOn => _layersNotToSpawnOn;
        public static LayerMask LayersToSpawnOn => _layersToSpawnOn;


        public static Vector2 GetValidSpawnPositionOnMap(in CompositeCollider2D spawnArea)
        {
            Vector2 spawnPos = GetRandomPointInCompositeCollider(in spawnArea);
            spawnPos = ConvertToValidSpawnPositionOnMap(spawnPos, in spawnArea);

            return spawnPos;
        }

        public static Vector2 ConvertToValidSpawnPositionOnMap(Vector2 spawnPosition, in CompositeCollider2D spawnArea)
        {
            for (int i = 0; i < _maxSpawnCheckAttempts; i++)
            {
                if (IsSpawnPositionValid(spawnPosition))
                    return spawnPosition;

                spawnPosition = GetRandomPointInCompositeCollider(in spawnArea);
            }

            Debug.LogWarning("Could not find valid spawn position.");

            return spawnPosition;
        }

        public static bool IsSpawnPositionValid(Vector2 spawnPosition)
        {
            Array.Clear(_collidersFound, 0, _collidersFound.Length);

            // Check if the spawn position overlaps with any colliders on the specified layermask
            int numCollidersFound = Physics2D.OverlapCircleNonAlloc(spawnPosition, _overlapCheckRadius,_collidersFound, _layersNotToSpawnOn);

            // Return true if no colliders are found, false otherwise
            return numCollidersFound == 0;
        }

        public static Vector2 GetRandomPointInCompositeCollider(in CompositeCollider2D collider)
        {
            int attempts = 0;
            Vector2 randomPoint;    
            Bounds bounds = collider.bounds;

            do
            {
                randomPoint = new Vector2(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y));
                attempts++;

            } while (!collider.OverlapPoint(randomPoint) && attempts < _maxSpawnCheckAttempts);

            return randomPoint;
        }

        public static Vector2 GetValidWormSegmentSpawnPositionForRandomChain(List<GameObject> disconnectedChain, in int chainIndex, float tangentCircleRadius)
        {
            // Get the position of the previous segment
            Vector2 centerOfTangentCircle = disconnectedChain[chainIndex - 1].transform.position;

            // Get the spawn position of the current segment
            Vector2 spawnPos = GetCandidateWormSegmentSpawnPositionForRandomChain(centerOfTangentCircle, tangentCircleRadius);

            // Convert to a valid spawn position on worm that does not overlap with other objects
            Vector2 validSpawnPos =
                ConvertToValidWormSegmentSpawnPosition(spawnPos, centerOfTangentCircle, tangentCircleRadius);

            return validSpawnPos;
        }

        public static Vector2 GetValidWormSegmentSpawnPositionForDetachedChain(List<GameObject> disconnectedChain, in int chainIndex, in int length, float tangentCircleRadius, WormCollectionsDataSO wormCollectionsSO)
        {
            // Get the position of the previous segmentindex
            Vector2 centerOfTangentCircle = disconnectedChain[chainIndex - 1].transform.position;

            // Get the spawn position of the current segment
            Vector2 spawnPos = GetCandidateWormSegmentSpawnPositionForDetachedChain(chainIndex, length, wormCollectionsSO);

            // Convert to a valid spawn position on worm that does not overlap with other objects
            Vector2 validSpawnPos =
                ConvertToValidWormSegmentSpawnPosition(spawnPos, centerOfTangentCircle, tangentCircleRadius);

            return validSpawnPos;
        }

        public static Vector2 GetCandidateWormSegmentSpawnPositionForRandomChain(Vector2 centerOfTangentCircle, float tangentCircleRadius)
        {
            Vector2 spawnPos;
            // If the chain was not part of the worm before, give a random position on circle with a random value on circle edge
            spawnPos = GetRandomPointOnCircle(centerOfTangentCircle, tangentCircleRadius);

            return spawnPos;
        }

        public static Vector2 GetCandidateWormSegmentSpawnPositionForDetachedChain(in int chainIndex, in int length, WormCollectionsDataSO wormCollectionsSO)
        {
            Vector2 spawnPos = Vector2.zero;

            return spawnPos;
        }
        public static Vector2 ConvertToValidWormSegmentSpawnPosition(Vector2 spawnPosition, Vector2 tangentCircleCenter, float tangentCircleRadius)
        {
            float collisionAngle = Mathf.Atan2(spawnPosition.y - tangentCircleCenter.y, spawnPosition.x - tangentCircleCenter.x);
            float deltaAngle = Mathf.PI / 180f;
            int maxAttempts = 360;
            Vector2 positionAroundWorm = spawnPosition;

            for (int i = 0; i < maxAttempts; i++)
            {
                // Increase the angle by deltaAngle collisionAngle += deltaAngle;
                collisionAngle += deltaAngle;
                // Wrap the angle to the range [-pi, pi]
                collisionAngle = Mathf.Repeat(collisionAngle + Mathf.PI, Mathf.PI * 2) - Mathf.PI;
                // Calculate the position on the circle using sine and cosine
                positionAroundWorm = GetPrecisePointOnCircle(tangentCircleCenter, tangentCircleRadius, collisionAngle);

                if (IsSpawnPositionValid(positionAroundWorm))
                {
                    //Debug.Log("Found Valid Position");
                    return positionAroundWorm;
                }
            }

            // Prevented re-attached worm segments from spawning inside walls or other objects
            // if a valid spawn position is not found after max attempts, by switching worm
            // segment's collider2d to a polygon collider that its shape is based on polygons 
            // instead of outlines like the primitive colliders 2D

            // If a valid spawn position is not found after max attempts, return last position
            // generated in the loop.
            Debug.LogWarning("Valid Spawn Position Not Found, Setting to default");
            return positionAroundWorm;
        }

        public static Vector2 GetPrecisePointOnCircle(Vector2 center, float radius, float currentThetaAngle)
        {
            return center + new Vector2(Mathf.Cos(currentThetaAngle), Mathf.Sin(currentThetaAngle)) * radius;
        }

        public static Vector2 GetRandomPointOnCircle(Vector2 center, float radius)
        {
            float angle = Random.Range(0, 2 * Mathf.PI);
            Vector2 position = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

            return position;
        }

        public static Quaternion GetRotationToLookAtTarget(Vector2 targetPos, Vector2 myPos)
        {
            targetPos.x -= myPos.x;
            targetPos.y -= myPos.y;

            float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;

            return Quaternion.Euler(new Vector3(0, 0, angle));
        }

        public static Vector2 GetHeadPosition(WormDataSO wormSO)
        {
            return wormSO.Head.transform.position;
        }
    }
}