using UnityEngine;
using UnityEngine.Events;
using WormGame.EventChannels;

namespace WormGame.EventChannels
{
    public class FloatFloatEventChannelListener : MonoBehaviour
    {

        [Header("Listen to Event Channels")]
        [SerializeField] private FloatFloatEventChannelSO _eventChannelSO;
        [Space]
        [Tooltip("Responds to receiving signal from Event Channel")]
        [SerializeField] private UnityEvent<float, float> _response;

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

        public void OnEventRaised(float value1, float value2)
        {
            if (_response != null)
                _response.Invoke(value1, value2);
        }

    }

}