using UnityEngine;
using WormGame.FactoryPattern;
using WormGame.Spawn;
using WormGame.Core.Traps;

namespace WormGame.Core.Traps
{
    public abstract class ArrowProduct : MonoBehaviour, IProduct
    {
        [SerializeField] protected string _productName = "Arrow Product";
        [SerializeField] protected ArrowDataSO _arrowDataSO;
        [SerializeField] protected WormDataSO _wormDataSO;
        [SerializeField] protected Rigidbody2D _rb2D;

        public string ProductName { get => _productName; set => _productName = value; }
        public GameObject GameObject => gameObject;

        public abstract void Initialize();

        protected Vector2 GetSpawnPosition()
        {
            SpawnAreaSO arrowSpawnArea = GetSpawnArea();

            float randomXPosition = Random.Range(arrowSpawnArea.MinXPosition, arrowSpawnArea.MaxXPosition);
            float randomYPosition = Random.Range(arrowSpawnArea.MinYPosition, arrowSpawnArea.MaxYPosition);

            return new Vector2(randomXPosition, randomYPosition);
        }

        private SpawnAreaSO GetSpawnArea()
        {
            int randomIndex = Random.Range(0, _arrowDataSO.ArrowSpawnAreas.Count);
            SpawnAreaSO arrowSpawnArea = _arrowDataSO.ArrowSpawnAreas[randomIndex];

            return arrowSpawnArea;
        }

        protected void SetVelocityTowardsFacingDirection(in Rigidbody2D rigidbody2D)
        {
            rigidbody2D.velocity = transform.right * _arrowDataSO.ArrowSpeed;
        }
    }
}