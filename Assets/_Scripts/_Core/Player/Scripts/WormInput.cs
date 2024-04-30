using UnityEngine;
using WormGame.EventChannels;
using WormGame.Managers;

namespace WormGame.Core.Player
{
    /// <summary>
    /// Informs the WormInputActions class
    /// </summary>
    public class WormInput : MonoBehaviour, IUpdatable
    {
        [SerializeField] private Worm _worm;
        private Vector3 _mouseWorldPosition;

        [Header("Broadcast on Event Channels")]
        [Tooltip("Notifies listeners that the first action button was pressed")]
        [SerializeField] private VoidEventChannelSO _firstActionButtonPressedSO;

        [Tooltip("Notifies listeners that the second action button was pressed")]
        [SerializeField] private VoidEventChannelSO _secondActionButtonPressedSO;

        public Vector3 MouseWorldPosition => _mouseWorldPosition;

        public void UpdateMe()
        {
            _mouseWorldPosition = GetMousePositionToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                OnFirstActionButtonPressed();
            }

            if (Input.GetMouseButtonDown(1))
            {
                OnSecondActionButtonPressed();
            }
        }

        private void OnFirstActionButtonPressed()
        {
            _firstActionButtonPressedSO.RaiseEvent();
        }

        private void OnSecondActionButtonPressed()
        {
            _secondActionButtonPressedSO.RaiseEvent();
        }

        private Vector3 GetMousePositionToWorldPoint(Vector3 mouseInput)
        {
            _mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseInput);

            return _mouseWorldPosition;
        }

        private void OnEnable()
        {
            UpdateManager.Instance.Register(this);
        }

        private void OnDisable()
        {
            UpdateManager.Instance.Unregister(this);
        }
    }
}