using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace WormGame.EventChannels
{
    public class IntVector2EventChannelListener : MonoBehaviour
    {
        /// <summary>
        /// This invokes a UnityEvent in response to receiving a specific event channel (Vector2EventChannelSO).
        /// This is a simple means of creating codeless interactivity (e.g. bounce sounds in the PaddleBall demo).
        /// </summary>

        [FormerlySerializedAs("")]
        [Header("Listen to Event Channels")]
        [SerializeField] private IntVector2EventChannelSO _eventChannelSO;
        [Space]
        [Tooltip("Responds to receiving signal from Event Channel")]
        [SerializeField] private UnityEvent<int, Vector2> _response;

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

        public void OnEventRaised(int i, Vector2 vector2)
        {
            if (_response != null)
                _response.Invoke(i, vector2);
        }
    }
}