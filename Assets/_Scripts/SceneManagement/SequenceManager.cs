using System.Collections;
using UnityEngine;
using WormGame.EventChannels;
using WormGame.Core.Utils;

namespace WormGame.SceneManagement
{

    /// <summary>
    /// This component uses a state machine to load different parts of the application.
    /// Use the "pre-load" time to show a splash screen and load any assets at startup.
    /// </summary>
    ///
    // TO-DO: use a ScriptableObject to define sequences, then use a state machine so that
    // each state can preload its own assets, raise events on enter and exit 

    public class SequenceManager : MonoBehaviour
    {

        // Inspector fields
        [Header("Preload (Splash Screen)")]
        [Tooltip("Prefab assets that load first.")]
        [SerializeField] private GameObject[] _preloadedAssets;
        [Tooltip("Minimal time in seconds to show splash screen")]
        [SerializeField] private float _preloadDelay = 2f;

        [Header("Broadcast on Event Channels")]
        [Tooltip("Update the loading progress value (percentage)")]
        [SerializeField] private FloatEventChannelSO _loadProgressUpdatedSO;
        [Tooltip("Notify listeners that preloading has finished.")]
        [SerializeField] private VoidEventChannelSO _preloadCompleteSO;

        [Header("Listen to Event Channels")]
        [SerializeField] private VoidEventChannelSO _applicationQuitSO;

        // MonoBehaviour event functions

        // Preloads any Prefabs that will be instantiated on start, then loads the next Scene.
        private void Start()
        {
            NullRefChecker.Validate(this);
            InstantiatePreloadedAssets();
            DelaySplashScreen(_preloadDelay);
        }

        // Subscribe to event channels
        private void OnEnable()
        {
            _applicationQuitSO.OnEventRaised += ExitApplication;
        }

        // Unsubscribe from event channels to prevent errors
        private void OnDisable()
        {
            _applicationQuitSO.OnEventRaised -= ExitApplication;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitApplication();
            }
        }
        // Methods

        // Use this to preload any assets
        private void InstantiatePreloadedAssets()
        {
            foreach (GameObject asset in _preloadedAssets)
            {
                Instantiate(asset);
            }

        }

        private void DelaySplashScreen(float delayInSeconds)
        {
            StartCoroutine(WaitForSplashScreenCoroutine(delayInSeconds));
        }

        private IEnumerator WaitForSplashScreenCoroutine(float delayInSeconds)
        {
            float endSplashScreenTime = Time.time + delayInSeconds;

            while (Time.time < endSplashScreenTime)
            {
                float progress = 100 * (1 - (endSplashScreenTime - Time.time) / delayInSeconds);
                progress = Mathf.Clamp(progress, 0f, 100f);

                OnLoadProgressUpdated(progress);

                yield return null;
            }

            OnShowMainMenu();
        }

        // Raises event channel during splash screen to update progress bar
        private void OnLoadProgressUpdated(float progressValue)
        {
            _loadProgressUpdatedSO.RaiseEvent(progressValue);
        }

        // Signals that preloading is complete
        private void OnShowMainMenu()
        {
            _preloadCompleteSO.RaiseEvent();
        }

        private void ExitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}