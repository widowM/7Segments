using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.EventChannels;

namespace WormGame
{
    public class PlayRandomEnemySound : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _enemySounds;
        [Header("Listen to Event Channels")]
        [SerializeField] private Vector2EventChannelSO _playEnemySoundAtPointSO;
        public void PlayRandomEnemySFX(Vector2 pos)
        {
            AudioClip randomEnemySound = _enemySounds[Random.Range(0, _enemySounds.Length)];
            AudioSource.PlayClipAtPoint(randomEnemySound, pos);
        }

        private void OnEnable()
        {
            _playEnemySoundAtPointSO.OnEventRaised += PlayRandomEnemySFX;
        }

        private void OnDisable()
        {
            _playEnemySoundAtPointSO.OnEventRaised -= PlayRandomEnemySFX;
        }
    }
}
