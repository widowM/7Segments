using UnityEngine;
using WormGame.EventChannels;
using WormGame.Core.Containers;
using DG.Tweening;
using WormGame.Variables;

namespace WormGame.Core.Enemies
{
    /// <summary>
    /// This class manages the current health of the enemy.
    /// </summary>
    /// <remarks>
    /// This class uses the Health component to store and modify the enemy's health value.
    /// It also provides methods to apply damage or healing effects.
    /// </remarks>
    /// 
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private ObjectPoolTagsContainerSO _objectPoolTagsContainerSO;
        [SerializeField] private Transform _spriteTransform;
        [SerializeField] private StringVariableSO _lootTag;

        private float _currentHealth = 3;

        [Header("Broadcast on Event Channels")]
        [SerializeField] private VoidEventChannelSO _enemyDyingSO;

        public void TakeDamage()
        {
            DecreaseHealth();
        }

        private void DecreaseHealth()
        {
            _currentHealth--;

            if (_currentHealth < 1)
            {
                OnEnemyDying();
                ObjectPooler.Instance.GetObject(_objectPoolTagsContainerSO.ImpactParticleTag, _spriteTransform.position, Quaternion.identity);

                GetComponent<Collider2D>().enabled = false;
                ObjectPooler.Instance.GetObject(_lootTag.Value, transform.position, Quaternion.identity);

                PlayEnemyDeathAnimation();
            }
        }

        private void PlayEnemyDeathAnimation()
        {
            DOTween.Sequence().Append(_spriteTransform.DORotate(new Vector3(0f, 0f, 720f), 1.5f, RotateMode.FastBeyond360)).
                    Join(_spriteTransform.DOScale(new Vector3(0, 0, 0), 1.5f)).SetEase(Ease.InSine).OnComplete(SendEnemyToPool);
        }

        private void OnEnemyDying()
        {
            _enemyDyingSO.RaiseEvent();
        }

        private void SendEnemyToPool()
        {
            string poolTag = GetComponent<PoolNameHolder>().PoolName.Value;
            ObjectPooler.Instance.ReturnObject(gameObject, poolTag);
        }

        private void InitializeHealth()
        {
            _currentHealth = Random.Range(_enemy.EnemySO.TotalHp, _enemy.EnemySO.TotalHp + 3);
        }

        private void OnEnable()
        {
            _spriteTransform.localScale = Vector3.one;
            _spriteTransform.rotation = Quaternion.identity;
            GetComponent<Collider2D>().enabled = true;
            InitializeHealth();
        }
    }
}