using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.EventChannels;

namespace WormGame
{
    public class SoulMeter : MonoBehaviour
    {
        [SerializeField] private int _soulsNeededToAdvance;
        private int _soulsPicked;
        private bool _missionAccomplished;
        [Header("Listen to Event Channels")]
        [SerializeField] private VoidEventChannelSO _pickedSoulSO;
        [Header("Broadcast on Event Channels")]
        [SerializeField] private FloatFloatEventChannelSO _fillingLevelMeterSO;
        [SerializeField] private VoidEventChannelSO _levelObjectiveCompletedSO;
        [SerializeField] private VoidEventChannelSO _playShortSuccessSFXSO;

        void Start()
        {
            _soulsPicked = 0;
        }

        private void PickedSoul()
        {
            _soulsPicked++;
            if (_soulsPicked >= _soulsNeededToAdvance && !_missionAccomplished)
            {
                Debug.Log("Gathered " + _soulsPicked + " , can advance.");
                _missionAccomplished = true;
                _levelObjectiveCompletedSO.RaiseEvent();
                _playShortSuccessSFXSO.RaiseEvent();
            }
            _fillingLevelMeterSO.RaiseEvent(_soulsPicked, _soulsNeededToAdvance);
        }

        private void OnEnable()
        {
            _pickedSoulSO.OnEventRaised += PickedSoul;
        }

        private void OnDisable()
        {
            _pickedSoulSO.OnEventRaised -= PickedSoul;
        }
    }
}
