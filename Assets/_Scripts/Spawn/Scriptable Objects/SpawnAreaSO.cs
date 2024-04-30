using UnityEngine;

namespace WormGame.Spawn
{
    /// <summary>
    /// This class inherits from ScriptableObject and defines a rectangular area for spawning gameobjects.
    /// It has four fields that represent the minimum and maximum x and y coordinates of the area.
    /// The class can be used to create different spawn areas for different types of gameobjects,
    /// such as enemies, power ups, obstacles, etc.
    /// </summary>

    [CreateAssetMenu(menuName = "ScriptableObjects/WormGame/SpawnArea")]
    public class SpawnAreaSO : ScriptableObject
    {
        [SerializeField] private float _minXSpawnPosition;
        [SerializeField] private float _maxXSpawnPosition;
        [SerializeField] private float _minYSpawnPosition;
        [SerializeField] private float _maxYSpawnPosition;

        public float MinXPosition
        {
            get { return _minXSpawnPosition; }
        }

        public float MaxXPosition
        {
            get { return _maxXSpawnPosition; }
        }

        public float MinYPosition
        {
            get { return _minYSpawnPosition; }
        }

        public float MaxYPosition
        {
            get { return _maxYSpawnPosition; }
        }
    }
}
