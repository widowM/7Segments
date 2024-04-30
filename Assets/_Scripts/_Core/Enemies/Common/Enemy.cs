using UnityEngine;
using WormGame.Core;
using WormGame.EventChannels;

namespace WormGame.Core.Enemies
{
    public abstract class Enemy : MonoBehaviour, IDamageable
    {
        // Fields
        [SerializeField] protected WormDataSO _wormDataSO;
        [SerializeField] protected GameplayDataSO _gameplayDataSO;
        [SerializeField] protected EnemySO _enemySO;
        [SerializeField] protected EnemyHealth _enemyHealth;
        [SerializeField] protected EnemyVisuals _enemyVisuals;
        [SerializeField] protected EnemyPhysics _enemyPhysics;
        [SerializeField] protected EnemyAI _enemyAI;
        [SerializeField] protected Collider2D _collider2D;
        
        [Header("Broadcast on Event Channels")]
        [SerializeField] protected VoidEventChannelSO _enemyDamagedSO;
        [SerializeField] protected VoidEventChannelSO _spawningEnemiesSO;
        [SerializeField] protected FloatFloatEventChannelSO _shakingCameraSO;
        [SerializeField] protected FloatEventChannelSO _pausingTimeForSecondsSO;
        
        [Header("Listen to Event Channels")]
        [SerializeField] protected VoidEventChannelSO _levelObjectiveCompletedSO;

        // Properties
        public EnemySO EnemySO => _enemySO;

        public virtual void TakeDamage(DamageInfo damageInfo)
        {
            OnShakingCamera();
            OnPausingTimeForSeconds(0.2f);
            OnEnemyDamaged();

            _enemyVisuals.ShowDamageEffect();
            _enemyPhysics.Knockback(damageInfo.projectileVelocity);
            _enemyHealth.TakeDamage();
        }

        private void OnShakingCamera()
        {
            _shakingCameraSO.RaiseEvent(_gameplayDataSO.CameraShakeIntensity, _gameplayDataSO.CameraShakeDuration);
        }

        private void OnPausingTimeForSeconds(float pauseDuration)
        {
            _pausingTimeForSecondsSO.RaiseEvent(pauseDuration);
        }

        private void OnEnemyDamaged()
        {
            _enemyDamagedSO.RaiseEvent();
        }

        private void OnLevelCompleted()
        {
            GetComponent<Collider2D>().enabled = false;
        }
        private void OnEnable()
        {
            _levelObjectiveCompletedSO.OnEventRaised += OnLevelCompleted;
        }

        private void OnDisable()
        {
            _levelObjectiveCompletedSO.OnEventRaised -= OnLevelCompleted;
        }
    }
}

