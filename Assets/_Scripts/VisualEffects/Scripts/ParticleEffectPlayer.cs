using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WormGame.VisualEffects
{
    public class ParticleEffectPlayer : MonoBehaviour
    {
        private bool _isPlaying;

        public void PlayStopEffect()
        {
            if (_isPlaying)
                GetComponent<ParticleSystem>().Stop();
            else
                GetComponent<ParticleSystem>().Play();
        }
    }
}