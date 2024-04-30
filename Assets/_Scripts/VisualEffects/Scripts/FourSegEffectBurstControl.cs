using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.EventChannels;

namespace WormGame
{
    public class FourSegEffectBurstControl : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _particleSystems;
        [SerializeField] private WaitForSeconds _zeroFiveSeconds = new WaitForSeconds(0.5f);
        [Header("Broadcast on Event Channels")]
        [SerializeField] private VoidEventChannelSO _playBurstSoundSO;

        public void StartBurst()
        {
            StartCoroutine(StartBursts());
        }

        IEnumerator StartBursts()
        {
            int burstsCount = 3;
            for (int i = 0; i < burstsCount; i++)
            {
                foreach (ParticleSystem particleSystem in _particleSystems)
                {
                    particleSystem.Emit(1);
                }

                // Raise sound event
                _playBurstSoundSO.RaiseEvent();
                yield return _zeroFiveSeconds;
            }
        }
    }
}
