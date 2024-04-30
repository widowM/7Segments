using UnityEngine;
using System.Collections;
using WormGame.FactoryPattern;

namespace WormGame.Spawn
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private Factory[] _factories;
        private Factory _factory;

        private WaitForSeconds _secondsBetweenSpawns = new WaitForSeconds(3);
        private bool _keepSpawning = true;

        private IEnumerator SpawnAtIntervals()
        {
            // Repeat until keepSpawning == false or this GameObject is disabled/destroyed.
            while (_keepSpawning)
            {
                // Put this coroutine to sleep until the next spawn time.
                yield return _secondsBetweenSpawns;

                // Now it's time to spawn again.
                Spawn();
            }
        }

        private void Spawn()
        {
            // Choose a random factory
            _factory = _factories[Random.Range(0, _factories.Length)];

            if (_factory != null)
            {
                _factory.GetProduct<IProduct>();
            }
        }

        private void OnEnable()
        {
            StartCoroutine(SpawnAtIntervals());
        }

        private void OnDisable()
        {
            _keepSpawning = false;
        }
    }
}