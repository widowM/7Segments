using System.Collections.Generic;
using UnityEngine;
using WormGame.Core.Containers;
using WormGame.EventChannels;
using DG.Tweening;

namespace WormGame.Core
{
    public abstract class WormSegmentPlayableAbstract : MonoBehaviour, IDamageable
    {
        [SerializeField] protected ObjectPoolTagsContainerSO _objectPoolTagsContainerSO;
        [SerializeField] protected WormDataSO _wormDataSO;
        [SerializeField] protected GameplayDataSO _gameplayDataSO;
        [SerializeField] protected WormCollectionsDataSO _wormCollectionsDataSO;
        [SerializeField] protected WormSegmentDataSO _segmentDataSO;
        [SerializeField] protected ParticleSystem _speedUpParticle;
        [SerializeField] protected List<SpriteRenderer> _spriteRends;
        private Tween _tween;
        [Header("Listen to Event Channels")]
        [SerializeField] protected VoidEventChannelSO _gotSpeedPowerUpSO;
        [SerializeField] protected VoidEventChannelSO _speedUpEffectEndedSO;

        [Header("Broadcast on Event Channels")]
        [SerializeField] protected Vector2EventChannelSO _spawningBloodSO;
        [SerializeField] protected VoidEventChannelSO _wormHitSO;
        [SerializeField] protected FloatFloatEventChannelSO _shakingCameraSO;
        [SerializeField] protected VoidEventChannelSO _gotDamagedSO;

        public WormDataSO WormDataSO => _wormDataSO;

        public virtual void TakeDamage(DamageInfo damageInfo)
        {
            ObjectPooler.Instance.
                GetObject(_objectPoolTagsContainerSO.ImpactParticleTag, damageInfo.hitPosition, Quaternion.identity);

            OnShakingCamera(_gameplayDataSO.CameraShakeIntensity, _gameplayDataSO.CameraShakeDuration);
            OnSpawningBlood(damageInfo.hitPosition);
            OnGotDamaged();
        }

        public abstract void ProcessDamage(DamageInfo damageInfo);

        private void OnShakingCamera(float camShakeIntensity, float camShakeDuration)
        {
            _shakingCameraSO.RaiseEvent(camShakeIntensity, camShakeDuration);
        }

        private void OnGotDamaged()
        {
            _gotDamagedSO.RaiseEvent();
        }

        private void OnSpawningBlood(Vector2 hitPosition)
        {
            _spawningBloodSO.RaiseEvent(hitPosition);
        }

        protected void PlaySpeedUpParticleEffect()
        {
            _speedUpParticle.Play();
        }

        protected void StopSpeedUpParticleEffect()
        {
            _speedUpParticle.Stop();
        }

        private void ResetColorAlpha()
        {
            foreach (var spriteRenderer in _spriteRends)
            {
                spriteRenderer.color = _segmentDataSO.Color;
            }
        }

        protected virtual void OnEnable()
        {
            _gotSpeedPowerUpSO.OnEventRaised += PlaySpeedUpParticleEffect;
            _speedUpEffectEndedSO.OnEventRaised += StopSpeedUpParticleEffect;
            _tween = transform.DOScale(1.05f, Random.Range(0.1f, 0.3f)).SetLoops(-1, LoopType.Yoyo);
        }

        protected virtual void OnDisable()
        {
            _gotSpeedPowerUpSO.OnEventRaised -= PlaySpeedUpParticleEffect;
            _speedUpEffectEndedSO.OnEventRaised -= StopSpeedUpParticleEffect;

            _tween.Kill();
            ResetColorAlpha();
        }
    }
}
