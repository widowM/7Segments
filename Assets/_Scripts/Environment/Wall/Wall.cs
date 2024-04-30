using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Core;
using WormGame.EventChannels;

namespace WormGame.Environment
{
    /// <summary>
    /// This class handles the behavior of the wall that can be damaged and destroyed on impact.
    /// </summary>
    /// <remarks>
    /// This class implements the IDamageable interface and the TakeDamage(Vector hitPosition) method.
    /// It checks if the player is powerupd to break the wall. If so, it destroys the wall and plays 
    /// a sound and a particle effect.
    /// </remarks>
    /// 
    public class Wall : MonoBehaviour, IDamageable
    {
        [SerializeField] private WormDataSO _wormDataSO;
        [SerializeField] private ParticleSystem _rubbleParticle;

        [Header("Broadcast on Event Channels")]
        [SerializeField] private VoidEventChannelSO _wallDestroyedSO;

        [SerializeField] private FloatFloatEventChannelSO _camShakeSO;
        [SerializeField] private FloatEventChannelSO _pausingTimeForSecondsSO;

        public void TakeDamage(DamageInfo damageInfo)
        {
            Instantiate(_rubbleParticle, transform.GetChild(0).transform.position, Quaternion.identity);
            OnWallDestroyed();
            Destroy(gameObject);
        }

        private void OnWallDestroyed()
        {
            _wallDestroyedSO.RaiseEvent();

            float camShakeIntensity = 7;
            float camShakeDuration = 0.3f;
            float pauseTimeDuration = 0.2f;

            _camShakeSO.RaiseEvent(camShakeIntensity, camShakeDuration);
            _pausingTimeForSecondsSO.RaiseEvent(pauseTimeDuration);
        }
    }
}