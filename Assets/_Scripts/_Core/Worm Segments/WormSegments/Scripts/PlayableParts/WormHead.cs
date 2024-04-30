using UnityEngine;
using WormGame.EventChannels;
using WormGame.Core.Utils;
using System.Collections;

namespace WormGame.Core
{

    /// <summary> This class is responsible for the handling of the worm's head that can be damaged by enemies.
    /// /// It uses the worm data and the event channels and broadcast events to other components,
    /// such as game over, blood spawning, and camera shaking.
    /// It also implements the IDamageable interface to handle the damage taken by the worm head
    /// and show the impact particle effect. I decided to separate segments to playable and non-playable, disconnected.
    /// </summary>

    public class WormHead : WormSegmentPlayableAbstract, IDamageable
    {
        [SerializeField] private Animator _biteAnimator;
        [SerializeField] private GameObject _biteObject;
        [Header("Listen to Event Channels")]
        [SerializeField] private VoidEventChannelSO _bitingStartedSO;
        [SerializeField] private VoidEventChannelSO _shootingFourSegsSO;
        [Header("Broadcast on Event Channels")]
        [SerializeField] private VoidEventChannelSO _gameOverSO;
        [SerializeField] private VoidEventChannelSO _gameOverSOScreenShownSO;
        [SerializeField] private VoidEventChannelSO _startSegBursts;

        public override void TakeDamage(DamageInfo damageInfo)
        {
            WormStateManager.Instance.GetState().TakeDamage(this, damageInfo);
        }

        public override void ProcessDamage(DamageInfo damageInfo)
        {
            // Process damage and handle game over logic
            base.TakeDamage(damageInfo);
            GameLog.GameplayInfoMessage("Game Over: |+|");
            WormStateManager.Instance.SetState(new IWormInvincibleState());
            OnGameOverScreenShown();
            OnGameOver();
        }

        private void StartBiting()
        {
            // Play biting animation
            _biteObject.SetActive(true);
            _biteAnimator.Play("Bite_Animation", -1, 0f);
            StartCoroutine(DisableAfterAnimation());
        }

        private void ShootFourSegs()
        {
            _startSegBursts.RaiseEvent();
        }

        private IEnumerator DisableAfterAnimation()
        {
            // Wait until the current animation is no longer playing
            while (_biteAnimator.GetCurrentAnimatorStateInfo(0).length >
                   _biteAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                yield return null;
            }

            // Set the game object to inactive after the animation has finished
            _biteObject.SetActive(false);
        }
        private void OnGameOverScreenShown()
        {
            _gameOverSOScreenShownSO.RaiseEvent();
        }

        private void OnGameOver()
        {
            _gameOverSO.RaiseEvent();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _bitingStartedSO.OnEventRaised += StartBiting;
            _shootingFourSegsSO.OnEventRaised += ShootFourSegs;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _bitingStartedSO.OnEventRaised -= StartBiting;
            _shootingFourSegsSO.OnEventRaised -= ShootFourSegs;
        }
    }
}