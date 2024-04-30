using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace WormGame.EventChannels
{
    public class IntEventChannelListener : MonoBehaviour
    {
        [FormerlySerializedAs("")]
        [Header("Listen to Event Channels")]
        [SerializeField] private IntEventChannelSO _eventChannelSO;
        [Space]
        [Tooltip("Responds to receiving signal from Event Channel")]
        [SerializeField] private UnityEvent<int> _response;

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

        public void OnEventRaised(int integer)
        {
            if (_response != null)
                _response.Invoke(integer);
        }
    }

}