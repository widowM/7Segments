using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace WormGame.EventChannels
{
    /// <summary>
    /// This invokes a UnityEvent in response to receiving a specific event channel (Vector2EventChannelSO).
    /// This is a simple means of creating codeless interactivity (e.g. bounce sounds in the PaddleBall demo).
    /// </summary>
    public class Vector2EventChannelListener : MonoBehaviour
    {
        [FormerlySerializedAs("")]
        [Header("Listen to Event Channels")]
        [SerializeField] private Vector2EventChannelSO _eventChannelSO;
        [Space]
        [Tooltip("Responds to receiving signal from Event Channel")]
        [SerializeField] private UnityEvent<Vector2> m_response;

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

        public void OnEventRaised(Vector2 vector2)
        {
            if (m_response != null)
                m_response.Invoke(vector2);
        }
    }
}