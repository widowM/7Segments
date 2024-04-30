using System.Collections.Generic;
using UnityEngine;
using System;
using WormGame.EventChannels;
using UnityEngine.SceneManagement;
using WormGame.Variables;

namespace WormGame.Core
{
    /// <summary>
    /// This class implements a generic object pooling system for Unity.
    /// It allows the creation and reuse of GameObjects from predefined _pools,
    /// reducing the overhead of instantiating and destroying objects at runtime.
    /// It supports multiple _pools of different types of objects, each with its
    /// own pool tag.It provides static methods for getting and returning pooled objects
    /// from their _pools in the specified position and rotation. In awake it is capable
    /// of spawning objects saved in Level setups json files made using the Level Editor tool.
    /// </summary>

    public class ObjectPooler : MonoBehaviour
    {

        [SerializeField] private bool _shouldExpandPools;
        [SerializeField] private bool _shouldSpawnSavedObjects;
        [SerializeField] private VoidEventChannelSO _worldGeneratedSO;
        private string _currentScene;

        // A dictionary that maps tags to lists of pooled objects
        private Dictionary<string, List<GameObject>> _pools;

        // A dictionary that maps prefabs to their tags
        private Dictionary<GameObject, string> _prefabTags;

        private GameObject _poolsParent;

        public static ObjectPooler Instance { get; private set; }

        // A list of _pools that can be edited in the inspector
        [SerializeField] private List<Pool> _poolsList;

        public void Initialize()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            

            CreatePools();
        }

        public void CreatePools()
        {
            if (Instance == null)
            {
                Debug.Log("Instance was null so we set it to this.");
                Instance = this;
            }

            // Check if there is another pool in the scene
            GameObject existingPool = GameObject.FindWithTag("POOLS");

            // If there is, destroy it
            if (existingPool != null)
            {
                Debug.Log("Deleting old _pools and creating new ones.");
                ClearPools();
                DestroyImmediate(existingPool);

                Debug.Log("Creating _pools from scratch.");
            }

            else
            {
                Debug.Log("Creating _pools from scratch. There wasn't an existing pool.");
            }

            InitializeDictionaries();
            _poolsParent = CreatePoolsParent();

            // Create the _pools from the list
            foreach (Pool pool in _poolsList)
            {
                CreatePool(pool.prefab, pool.tag, pool.size, _poolsParent);
            }

            if (_shouldSpawnSavedObjects && Application.isPlaying)
            {
                LevelEditorUtils.GenerateSavedChains();
                LevelEditorUtils.SpawnSavedLevelGameObjects();
                OnWorldGenerated();
            }
        }

        private void CreatePool(in GameObject prefab, in StringVariableSO tag, in int size, in GameObject parent)
        {
            if (_pools.ContainsKey(tag.Value))
            {
                Debug.LogError("A pool with the same tag already exists." +tag.Value);
                return;
            }

            if (_prefabTags.ContainsKey(prefab))
            {
                Debug.LogError("The prefab already has a tag assigned.");
                return;
            }
            GameObject poolParent = new GameObject();
            poolParent.name = tag.Value + " Pool";
            poolParent.transform.SetParent(parent.transform);
            
            List<GameObject> pool = new List<GameObject>();

            // Instantiate the prefabs and add them to the pool
            for (int i = 0; i < size; i++)
            {
                GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                obj.SetActive(false);
                obj.transform.SetParent(poolParent.transform, true);
                pool.Add(obj);
            }

            // Add the pool and the tag to the dictionaries
            _pools.Add(tag.Value, pool);
            _prefabTags.Add(prefab, tag.Value);
        }

        private void OnWorldGenerated()
        {
            _worldGeneratedSO.RaiseEvent();
            Debug.Log("World Generated Raised");
        }

        public GameObject GetObject(string tag, Vector3 position, Quaternion rotation)
        {
            if (_pools == null)
            {
                throw new NullReferenceException("/ / / Create Pools first / / /");
            }

            if (!_pools.ContainsKey(tag))
            {
                Debug.LogError("No pool with this tag exists.");
                return null;
            }

            List<GameObject> pool = _pools[tag];

            // Loop through the pool and find an inactive object
            for (int i = 0; i < pool.Count; i++)
            {
                GameObject obj = pool[i];

                if (!obj.activeInHierarchy)
                {
                    // Activate the object and return it
                    obj.SetActive(true);
                    obj.transform.position = position;
                    obj.transform.rotation = rotation;

                    return obj;
                }
            }

            if (_shouldExpandPools)
            {
                // No inactive object is found, so expand the pool
                GameObject newObj = ExpandPool(tag, pool);
                newObj.SetActive(true);
                newObj.transform.position = position;
                newObj.transform.rotation = rotation;
                return newObj;
            }

            // If no inactive object is found, return null
            Debug.LogWarning("No object available in this pool.");

            return null;
        }

        public GameObject GetObject(string tag)
        {
            if (_pools == null)
            {
                // Throw a custom exception with a message
                throw new NullReferenceException("/ / / You should Create Pools first / / /");
            }

            if (!_pools.ContainsKey(tag))
            {
                Debug.LogWarning("No pool with this tag exists.");
                return null;
            }

            List<GameObject> pool = _pools[tag];

            // Loop through the pool and find an inactive object
            for (int i = 0; i < pool.Count; i++)
            {
                GameObject obj = pool[i];

                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true);

                    return obj;
                }
            }

            if (_shouldExpandPools)
            {
                // No inactive object is found, so expand the pool
                GameObject newObj = ExpandPool(tag, pool);
                newObj.SetActive(true);

                return newObj;
            }

            // If no inactive object is found, return null
            Debug.LogWarning("No object available in this pool.");

            return null;
        }

        public void ReturnObject(in GameObject obj, in string tag)
        {
            if (!_pools.ContainsKey(tag))
            {
                Debug.LogError("No pool with this tag exists.");
                return;
            }

            List<GameObject> pool = _pools[tag];

            if (!pool.Contains(obj))
            {
                Debug.LogError("The object does not belong to this pool.");
                return;
            }

            obj.SetActive(false);
            pool.Add(obj);
        }

        public void ReturnObject(in GameObject obj)
        {
            if (!_prefabTags.ContainsKey(obj))
            {
                Debug.LogError("The object does not have a prefab tag assigned.");
                return;
            }

            string tag = _prefabTags[obj];

            ReturnObject(obj, tag);
        }

        public void ClearPools()
        {
            // Check if there is another pool in the scene
            GameObject existingPool = GameObject.FindWithTag("POOLS");
            _pools = null;
            if (existingPool != null)
            {
                Debug.Log("Deleting _pools.");

                DestroyImmediate(existingPool);          
            }

            else
            {
                Debug.Log("No _pools to clear.");
            }
        }

        private void InitializeDictionaries()
        {
            _pools = new Dictionary<string, List<GameObject>>();
            _prefabTags = new Dictionary<GameObject, string>();
        }

        private GameObject CreatePoolsParent()
        {
            _poolsParent = new GameObject();
            _poolsParent.name = "POOLS";
            _poolsParent.tag = "POOLS";

            return _poolsParent;
        }

        private GameObject ExpandPool(string tag, List<GameObject> pool)
        {
            // Find the pool type based on the tag
            Pool poolType = _poolsList.Find(p => p.tag.Value == tag);

            GameObject newObj = Instantiate(poolType.prefab);
            newObj.SetActive(false);

            // Set the parent of the new object to be the same as the other objects in the pool
            newObj.transform.SetParent(_poolsParent.transform, false);

            pool.Add(newObj);

            Debug.Log($"Expanded pool for tag {tag}. New pool size: {pool.Count}");

            return newObj;
        }
    }
}