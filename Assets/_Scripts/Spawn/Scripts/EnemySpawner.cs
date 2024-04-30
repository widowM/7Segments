using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using WormGame.FactoryPattern;
using WormGame.EventChannels;
using WormGame.Core;
using WormGame.Core.Utils;
using WormGame.Managers;

namespace WormGame.Spawn
{

    /// <summary>
    /// This class is responsible for getting enemies from the EnemyConcreteFactory.
    /// It uses the factory design pattern to create different types of enemies based
    /// on the level and difficulty. For testing reasons, we have an update to check
    /// for e key pressed to spawn an enemy in a random position in the tilemap collider2d.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Factory _factory;

        [SerializeField] private LayerMask _spawnMask;

        [SerializeField] private CompositeCollider2D _spawnableAreaCol;

        [SerializeField] private GameObject _enemyPrefab;

        [Header("Listen to Event Channels")]
        [SerializeField] private VoidEventChannelSO _spawningEnemySO;


        private void Spawn()
        {
            Vector2 validSpawnPosition = SpawnUtils.GetValidSpawnPositionOnMap(in _spawnableAreaCol);

            Vector2[] args = { validSpawnPosition };
            if (_factory != null)
            {
                _factory.GetProduct<IProduct>(args);
            }
        }

        private void OnEnable()
        {
            _spawningEnemySO.OnEventRaised += Spawn;
        }

        private void OnDisable()
        {
            _spawningEnemySO.OnEventRaised -= Spawn;
        }
    }
}