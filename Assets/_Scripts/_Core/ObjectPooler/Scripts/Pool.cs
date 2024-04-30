// A serializable class to store the pool data
using UnityEngine;
using WormGame.Variables;

namespace WormGame.Core
{
    [System.Serializable]
    public class Pool
    {
        public GameObject prefab; // The prefab to pool
        public StringVariableSO tag; // The tag to identify the pool
        public int size; // The size of the pool
    }
}