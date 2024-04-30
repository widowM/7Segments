using UnityEngine;
using UnityEngine.UI;
using WormGame.EventChannels;
using WormGame.Core.Utils;

namespace WormGame.UI
{

    /// <summary>
    /// This specialized UI screen can cover the  
    /// </summary>
    public class SplashScreen : MonoBehaviour
    {
        [Tooltip("Loading bar progress")]
        [SerializeField] private Slider _progressBar;
        [Header("Listen to Event Channels")]
        [Tooltip("Percentage of loading time that is complete")]
        [SerializeField] private FloatEventChannelSO _loadProgressUpdatedSO;
        [Tooltip("Is the loading complete?")]
        [SerializeField] private VoidEventChannelSO _preloadCompleteSO;

        private void Awake()
        {
            NullRefChecker.Validate(this);
        }

        private void OnEnable()
        {
            _loadProgressUpdatedSO.OnEventRaised += UpdateProgressBar;
            _preloadCompleteSO.OnEventRaised += Hide;
        }

        private void OnDisable()
        {
            _loadProgressUpdatedSO.OnEventRaised -= UpdateProgressBar;
            _preloadCompleteSO.OnEventRaised -= Hide;
        }

        private void UpdateProgressBar(float value)
        {
            _progressBar.value = value;
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}