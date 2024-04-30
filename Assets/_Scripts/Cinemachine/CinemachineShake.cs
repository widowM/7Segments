using UnityEngine;
using Cinemachine;
using WormGame.EventChannels;
using WormGame.Core;
using WormGame.Managers;

namespace WormGame.Cinemachine
{
    /// <summary>
    /// A class responsible for camera shake effects.
    /// </summary>
    public class CinemachineShake : MonoBehaviour, IUpdatable
    {
        public static CinemachineShake Instance { get; private set; }
        private CinemachineVirtualCamera _cinemachineVirtualCamera;
        private float _shakeTimer = 0;
        private float _startingIntensity;
        private float _shakeTimerTotal;

        [Header("Listen to Event Channels")]
        [SerializeField] private FloatFloatEventChannelSO _shakeCamera;

        private void Awake()
        {
            Instance = this;
            _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();

            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        }

        public void UpdateMe()
        {
            if (_shakeTimer > 0)
            {
                _shakeTimer -= Time.unscaledDeltaTime;

                if (_shakeTimer <= 0f)
                {
                    // Time over!
                    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                        _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                    cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
                        Mathf.Lerp(_startingIntensity, 0f, 1 - _shakeTimer / _shakeTimerTotal);
                }
            }
        }

        private void ShakeCamera(float intensity, float duration)
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
            _startingIntensity = intensity;
            _shakeTimerTotal = duration;
            _shakeTimer = duration;
        }

        private void OnEnable()
        {
            _shakeCamera.OnEventRaised += ShakeCamera;
            UpdateManager.Instance.Register(this);
        }
        private void OnDisable()
        {
            _shakeCamera.OnEventRaised -= ShakeCamera;
            UpdateManager.Instance.Unregister(this);
        }
    }
}