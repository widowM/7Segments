using System.Collections.Generic;
using UnityEngine;
using WormGame.EventChannels;
using WormGame;

namespace WormGame.Managers
{
    /// <summary>
    /// This struct pairs an event channel with a Screen. 
    /// </summary>
    [System.Serializable]
    public struct ScreenDisplayEventChannel
    {
        [Tooltip("Signal to listen for")]
        public VoidEventChannelSO eventChannel;

        [Tooltip("UI Screen to enable")]
        public GameObject screenToDisplay;
    }

    /// <summary>
    /// The UI Manager manages the UI screens (View base class) using event channels
    /// paired with each Screen (canvas).
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Tooltip("Default starting modal screen (e.g. main menu)")]
        [SerializeField] private GameObject _homeScreen;

        [Header("Listen to Event Channels")]
        [Tooltip("Notification to show the Home Screen")]
        [SerializeField] private VoidEventChannelSO _homeScreenShown;

        [Space]
        [Tooltip("Displays the specified View when receiving notification via the event channel")]
        [SerializeField] private List<ScreenDisplayEventChannel> _displayScreenOnEvent;

        // The currently active Screen
        private GameObject _currentScreen;

        // A list of all Screens (canvases) to show/hide
        private List<GameObject> _screens = new List<GameObject>();


        // Subscribes to event channels:
        //
        // - Shows each Screen(canvas) when raising the event channel (via a lambda as a callback)
        // - _homeScreenShown shows the default screen canvas

        private void OnEnable()
        {
            foreach (ScreenDisplayEventChannel channel in _displayScreenOnEvent)
            {
                channel.eventChannel.OnEventRaised += () => Show(channel.screenToDisplay);
            }

            _homeScreenShown.OnEventRaised += ShowHomeScreen;
            GetComponent<Canvas>().worldCamera = Camera.main;
        }


        // Unregister the callback to prevent errors
        private void OnDisable()
        {
            foreach (ScreenDisplayEventChannel channel in _displayScreenOnEvent)
            {
                channel.eventChannel.OnEventRaised -= () => Show(channel.screenToDisplay);
            }

            _homeScreenShown.OnEventRaised -= ShowHomeScreen;
        }

        private void Awake()
        {
            Initialize();
        }

        // Clears history and hides all Views except the home screen
        private void Initialize()
        {
            RegisterViews();

            ShowHomeScreen();
        }

        // Store each Screen from _displayScreenOnEvent into the master list of Screens
        private void RegisterViews()
        {
            if (_homeScreen != null && !_screens.Contains(_homeScreen))
                _screens.Add(_homeScreen);

            foreach (ScreenDisplayEventChannel channel in _displayScreenOnEvent)
            {
                if (channel.screenToDisplay != null && !_screens.Contains(channel.screenToDisplay))
                    _screens.Add(channel.screenToDisplay);
            }
        }

        // Shows the homescreen
        private void ShowHomeScreen()
        {
            if (_homeScreen == null)
                return;

            foreach (GameObject screen in _screens)
            {
                screen.SetActive(false);
            }

            _homeScreen.SetActive(true);
            _currentScreen = _homeScreen;
        }


        // Shows the corresponding screen(canvas) based on the event raised
        public void Show(GameObject screen)
        {
            if (screen == null)
                return;

            foreach (GameObject uiScreen in _screens)
            {
                uiScreen.SetActive(false);
            }

            screen.SetActive(true);
            _currentScreen = screen;
        }
    }
}