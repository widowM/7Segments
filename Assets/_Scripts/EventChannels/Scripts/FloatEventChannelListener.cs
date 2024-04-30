using UnityEngine;
using UnityEngine.Events;

namespace WormGame.EventChannels
{
    public class FloatEventChannelListener : MonoBehaviour
    {

        [Header("Listen to Event Channels")]
        [SerializeField] private FloatEventChannelSO _eventChannelSO;
        [Space]
        [Tooltip("Responds to receiving signal from Event Channel")]
        [SerializeField] private UnityEvent<float> _response;

        private void OnEnable()
        {
            if (_eventChannelSO != null)
                _eventChannelSO.OnEventRaised += OnEventRaised;
        }

        private void OnDisable()
        {
            if (_eventChannelSO != null)
                _eventChannelSO.OnEventRaised -= OnEventRaised;
        }

        public void OnEventRaised(float value)
        {
            if (_response != null)
                _response.Invoke(value);
        }
    }
}
