using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.EventChannels;
using WormGame.Core;
using WormGame.Core.Utils;

namespace WormGame.Managers
{
    /// <summary>
    /// This controls the flow of gameplay in the WormGame. The GameManager notifies listeners of
    /// game states. 
    /// </summary>

    public class GameManager : MonoBehaviour
    {
        [Tooltip("Starts the game automatically when loading scene")]
        [SerializeField] private bool _autoStart = false;

        [Header("Broadcast on Event Channels")]
        [Tooltip("Begin GameplayDataSO")]
        [SerializeField] private VoidEventChannelSO _gameStartedSO;
        

        [Tooltip("End GameplayDataSO")]
        [SerializeField] private VoidEventChannelSO _gameEndedSO;

        [Tooltip("Score point and update UI")]

        [SerializeField] private VoidEventChannelSO _sceneUnloadedSO;

        [Tooltip("Notifies UIs to close all screens and go back to home screen")]
        [SerializeField] private VoidEventChannelSO _homeScreenShownSO;

        [Tooltip("Notifies UIs to close all screens and only show the GameplayDataSO screen")]
        [SerializeField] private VoidEventChannelSO _gameplayScreenShownSO;


        [SerializeField] private VoidEventChannelSO _startCreatingLevel;

        [Header("Listen to Event Channels")]

        [SerializeField] private VoidEventChannelSO _worldGeneratedSO;

        [Tooltip("All objectives are complete")]
        [SerializeField] private VoidEventChannelSO _allObjectivesCompletedSO;

        [Tooltip("Reset the score and stop GameplayDataSO. Begins GameplayDataSO if AutoStart not enabled")]
        [SerializeField] private VoidEventChannelSO _gameResetSO;

        [Tooltip("Is the game time paused?")]
        [SerializeField] private BoolEventChannelSO _isPausedSO;

        [Tooltip("Notifies listeners to go back to main menu scene")]
        [SerializeField] private VoidEventChannelSO _gameQuitSO;

        [Tooltip("Notifies listeners that the scene loaded asynchronously is set to active")]
        [SerializeField] private VoidEventChannelSO _asyncSceneLoadCompletedSO;
        private bool _isGameOver;

        // Subscribe to event channels
        private void OnEnable()
        {
            _allObjectivesCompletedSO.OnEventRaised += OnAllObjectivesCompleted;
            _gameResetSO.OnEventRaised += ResetGame;
            _isPausedSO.OnEventRaised += PauseGame;
            _gameQuitSO.OnEventRaised += OnUnloadScene;
            _asyncSceneLoadCompletedSO.OnEventRaised += Initialize;
            _worldGeneratedSO.OnEventRaised += StartGame;
        }

        // Unsubscribe from event channels to prevent errors
        private void OnDisable()
        {
            _allObjectivesCompletedSO.OnEventRaised -= OnAllObjectivesCompleted;
            _gameResetSO.OnEventRaised -= ResetGame;
            _isPausedSO.OnEventRaised -= PauseGame;
            _gameQuitSO.OnEventRaised -= OnUnloadScene;
            _asyncSceneLoadCompletedSO.OnEventRaised -= Initialize;
            _worldGeneratedSO.OnEventRaised -= StartGame;
        }

        // Plays the game automatically if _autoStart is enabled
        //private void Start()
        //{
        //    Initialize();
        //    if (_autoStart)
        //        StartGame();
        //}

        // Checks if we are missing any necessary components/assets/dependencies to play the game. Passes dependences
        // to the m_GameSetup and then sets up the walls, ball, and paddles.
        private void Initialize()
        {
            NullRefChecker.Validate(this);
            _startCreatingLevel.RaiseEvent();
            _gameplayScreenShownSO.RaiseEvent();

            //StartGame();
            //m_GameSetup.Initialize(m_GameData, m_InputReader);
            //m_GameSetup.SetupLevel();
        }

        // If the dependencies are initialized properly, start the game and notify
        // any listeners

        // CHANGED FROM PUBLIC TO PRIVATE //
        private void StartGame()
        {
            _gameStartedSO.RaiseEvent();
            //_gameplayScreenShownSO.RaiseEvent();
            //_worldGeneratedSO.RaiseEvent();
        }

        // Alternatively, use the expression-bodied syntax:
        //public void StartGame() => _gameStartedSO.RaiseEvent();

        // Sets the game over flag and notifies any listeners
        public void EndGame()
        {
            _isGameOver = true;
            _gameEndedSO.RaiseEvent();
        }

        // Sets the game over flag to false and calls StartGame.
        private void ResetGame()
        {
            _isGameOver = false;
            StartGame();
        }

        private void PauseGame(bool state)
        {
            Time.timeScale = (state) ? 0 : 1;
        }

        // Ends the game if all objectives are complete
        private void OnAllObjectivesCompleted()
        {
            _isGameOver = true;
            _gameEndedSO.OnEventRaised();
        }

        // Restarts the game from the win screen input. Ignored if not already finished
        // playing. 
        private void OnReplay()
        {
            if (!_isGameOver)
                return;

            _gameStartedSO.RaiseEvent();
            _isGameOver = false;
        }

        private void OnUnloadScene()
        {
            _sceneUnloadedSO.RaiseEvent();
            _homeScreenShownSO.RaiseEvent();
        }
    }
}