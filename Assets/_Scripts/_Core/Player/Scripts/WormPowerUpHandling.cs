using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Core.Utils;
using WormGame.EventChannels;

namespace WormGame.Core.Player
{
    /// <summary> 
    /// Worm Power-up Handler
    /// </summary>
    public class WormPowerUpHandling : MonoBehaviour
    {
        [SerializeField] private Worm _worm;

        private float _speedMultiplier;
        private float _currentPowerUpTime;
        private float _extraChainingPowerUpDuration = 5f;
        private bool _isSpedUp;
        private readonly WaitForSeconds _invincibilityCountdown = new WaitForSeconds(5);
        [SerializeField] private Material _powerUpImageEffect;

        [Header("Listen to Event Channels")]
        [SerializeField] private VoidEventChannelSO _gotSpeedPowerUpSO;
        [SerializeField] private VoidEventChannelSO _gotInvincibilitySO;
        [SerializeField] private VoidEventChannelSO _got4SegsSO;
        [Header("Broadcast on Event Channels")]
        [SerializeField] private VoidEventChannelSO _speedUpEffectEndedSO;
        [SerializeField] private VoidEventChannelSO _playingInvincibleWormAnimationSO;
        [SerializeField] private VoidEventChannelSO _stoppingInvincibleWormAnimationSO;
        [SerializeField] private VoidEventChannelSO _shootingFourSegs;

        private void ApplySpeedPowerUpEffects()
        {
            if (_isSpedUp)
            {
                _currentPowerUpTime += _extraChainingPowerUpDuration;
            }
            else
            {
                StartCoroutine(SpeedUpWormCoroutine());
            }
        }

        private void ApplyInvincibility()
        {
            StartInvincibilityCountdown();
        }

        // TODO: Find a way to replace boolean flags with IWormState logic
        private IEnumerator SpeedUpWormCoroutine()
        {
            SpeedUpWorm();
            _worm.WormDataSO.IsPowerUpd = true;

            _currentPowerUpTime += _extraChainingPowerUpDuration;

            while (_currentPowerUpTime > 0)
            {
                // Check if power-up time is less than 3 seconds
                if (_currentPowerUpTime < _extraChainingPowerUpDuration)
                {
                    // Interpolate the value from initial to target
                    float t = Mathf.Clamp01(_currentPowerUpTime / 3f);

                    _powerUpImageEffect.SetFloat("_VignetteIntensity", 1.55f * t);
                }
                else if (_currentPowerUpTime > _extraChainingPowerUpDuration)
                {
                    // Keep the value steady at the initial value
                    _powerUpImageEffect.SetFloat("_VignetteIntensity", 1.55f);
                }
                else
                {
                    _powerUpImageEffect.SetFloat("_VignetteIntensity", 0);
                }
                _currentPowerUpTime -= Time.deltaTime;
                yield return null;
            }

            SpeedDownWorm();
            _worm.WormDataSO.IsPowerUpd = false;
        }


        private void SpeedUpWorm()
        {
            WormPhysicsDataSO wormPhysicsSO = _worm.WormPhysicsDataSO;
            _speedMultiplier = 2;
            wormPhysicsSO.WormMovementSpeed *= _speedMultiplier;
            _isSpedUp = true;
        }

        private void SpeedDownWorm()
        {
            OnSpeedUpEffectEnded();
            WormPhysicsDataSO wormPhysicsSO = _worm.WormPhysicsDataSO;
            _speedMultiplier = 0.5f;
            wormPhysicsSO.WormMovementSpeed *= _speedMultiplier;
            _isSpedUp = false;
        }

        private void OnSpeedUpEffectEnded()
        {
            _speedUpEffectEndedSO.RaiseEvent();
        }

        private void StartInvincibilityCountdown()
        {
            WormStateManager.Instance.SetState(new IWormInvincibleState());
            StartCoroutine(TemporaryInvincibilityCoroutine());
        }

        private IEnumerator TemporaryInvincibilityCoroutine()
        {
            OnPlayingInvincibleWormAnimation();
            DOTween.Sequence().Append(DOTween.To(() => 0, value => _powerUpImageEffect.SetFloat("_VignetteIntensity", value), 1.55f, 1))
                .Append(DOTween.To(() => 1.363f, value => _powerUpImageEffect.SetFloat("_VignetteIntensity", value), 0, 4))            
            .SetEase(Ease.InOutQuad);

            yield return _invincibilityCountdown;

            WormStateManager.Instance.SetState(new IWormNormalState());
            GameLog.GameplayInfoMessage("Worm is not invincible anymore");

            OnStoppingInvincibleWormAnimation();
        }

        private void OnPlayingInvincibleWormAnimation()
        {
            _playingInvincibleWormAnimationSO.RaiseEvent();
        }

        private void OnStoppingInvincibleWormAnimation()
        {
            _stoppingInvincibleWormAnimationSO.RaiseEvent();
        }
        
        private void ApplyFourSegsPowerUp()
        {
            _shootingFourSegs.RaiseEvent();
        }

        // TODO: Fix this to play and stop the correct particle system
        private void OnEnable()
        {
            _gotSpeedPowerUpSO.OnEventRaised += ApplySpeedPowerUpEffects;
            _gotInvincibilitySO.OnEventRaised += ApplyInvincibility;
            _got4SegsSO.OnEventRaised += ApplyFourSegsPowerUp;
        }

        private void OnDisable()
        {
            _gotSpeedPowerUpSO.OnEventRaised -= ApplySpeedPowerUpEffects;
            _gotInvincibilitySO.OnEventRaised -= ApplyInvincibility;
            _got4SegsSO.OnEventRaised -= ApplyFourSegsPowerUp;
            _powerUpImageEffect.SetFloat("_VignetteIntensity", 0);
        }
    }
}