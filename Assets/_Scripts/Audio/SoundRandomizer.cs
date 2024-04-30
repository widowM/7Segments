using UnityEngine;

namespace WormGame.Audio
{
    /// <summary>
    /// A class that randomizes the volume and pitch of an audio source before playing a sound.
    /// </summary>
    public class SoundRandomizer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private float _minPitch;
        [SerializeField] private float _maxPitch;
        [SerializeField] private float _minVolume;
        [SerializeField] private float _maxVolume;

        // Properties
        public float MinPitch
        {
            set { _minPitch = Mathf.Clamp(value, 0, 1); }
        }
        public float MaxPitch
        {
            set { _maxPitch = Mathf.Clamp(value, 0, 1); }
        }
        public float MinVolume
        {
            set { _minVolume = Mathf.Clamp(value, 0, 1); }
        }
        public float MaxVolume
        {
            set { _maxVolume = Mathf.Clamp(value, 0, 1); }
        }

        // Methods
        public void RandomizePitch()
        {
            _audioSource.pitch = Random.Range(_minPitch, _maxPitch);
        }

        public void RandomizeVolume()
        {
            _audioSource.volume = Random.Range(_minVolume, _maxVolume);
        }
    }
}
