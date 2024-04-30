using UnityEngine;
using WormGame.EventChannels;
using WormGame.Core.Utils;

namespace WormGame.UI
{
    /// <summary>
    /// This class connects a UI Button to an event channel that takes no parameters.
    /// </summary>
    public class ButtonVoidEventChannelBinder : MonoBehaviour
    {
        [Header("Broadcast on Event Channel")]
        [Tooltip("The event channel to raise.")]
        [SerializeField] private VoidEventChannelSO _eventChannel;
        [Space]
        [Tooltip("Cooldown window between button clicks.")]
        [SerializeField] private float _delay = 0.5f;

        private float _timeToNextEvent;

        // Valid dependencies (m_Button or _eventChannel) and log an error if missing
        private void Awake()
        {
            NullRefChecker.Validate(this);
        }

        //private void OnEnable()
        //{
        //    m_Button.onClick.AddListener(RaiseEvent);
        //}

        //private void OnDisable()
        //{
        //    m_Button.onClick.RemoveListener(RaiseEvent);
        //    //m_Button.clicked -= RaiseEvent;
        //}

        public void RaiseEvent()
        {
            if (Time.time < _timeToNextEvent)
                return;

            _eventChannel.RaiseEvent();
            _timeToNextEvent = Time.time + _delay;
        }

    }
}