using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.EventChannels;

namespace WormGame.TimeHandling
{
    /// <summary>
    /// This class is responsible for handling the time scale of the game.
    /// It listens to an event channel that passes a float value as a parameter.
    /// It uses a coroutine to pause the time scale for a given number of seconds,
    /// and then resumes it to normal.
    /// </summary>
    public class TimeHandler : MonoBehaviour
    {
        [Header("Listen to Event Channels")]
        [SerializeField] private FloatEventChannelSO _pauseTimeForSeconds;

        private void Awake()
        {
            Time.timeScale = 1;
        }
        private void PauseTime(float seconds)
        {
            StartCoroutine(PauseTimeForSeconds(seconds));
        }

        private IEnumerator PauseTimeForSeconds(float seconds)
        {
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(seconds);

            Time.timeScale = 1;
        }
        private void OnEnable()
        {
            _pauseTimeForSeconds.OnEventRaised += PauseTime;
        }

        private void OnDisable()
        {
            _pauseTimeForSeconds.OnEventRaised -= PauseTime;
        }
    }
}