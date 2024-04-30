using UnityEngine;
using WormGame.Core;
using WormGame.Managers;

namespace WormGame.Audio
{
    public class FootstepSoundPlayer : MonoBehaviour//, IUpdatable
    {
        // Fields
        [SerializeField] private WormDataSO _wormDataSO;
        [SerializeField] private AudioClip[] _footsteps;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private float _minPitch = 0.6f;
        [SerializeField] private float _maxPitch = 0.8f;
        [SerializeField] private float _timeBetweenFootstepsInSeconds = 3;
        private float _currentTime = 0;

        private void PlaySlugStepSound()
        {
            _currentTime -= Time.deltaTime;

            if (_currentTime <= 0)
            {
                int randomFootstepsIndex = Random.Range(0, _footsteps.Length - 1);
                AudioClip footstep = GetRandomFootstepClip(randomFootstepsIndex);
                _audioSource.pitch = Random.Range(_minPitch, _maxPitch);
                _audioSource.PlayOneShot(footstep);
                ResetCurrentTime();
            }
        }

        private AudioClip GetRandomFootstepClip(int randomFootstepsIndex)
        {
            return _footsteps[randomFootstepsIndex];
        }

        private void ResetCurrentTime()
        {
            _currentTime = _timeBetweenFootstepsInSeconds;
        }

        //private void OnEnable()
        //{
        //    UpdateManager.Instance.Register(this);
        //}

        //private void OnDisable()
        //{
        //    UpdateManager.Instance.Unregister(this);
        //}
    }
}
