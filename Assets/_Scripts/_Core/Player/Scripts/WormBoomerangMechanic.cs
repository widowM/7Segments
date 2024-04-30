using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.EventChannels;
using static UnityEngine.Rendering.HableCurve;

namespace WormGame.Core.Player
{
    public class WormBoomerangMechanic : MonoBehaviour
    {
        [SerializeField] private WormMechanicHelper _wormMechanicHelper;
        [Header("Broadcast on Event Channels")]
        [SerializeField] private FloatEventChannelSO _pauseTimeForSecondsSO;
        [Header("Listen to Event Channels")]
        [SerializeField] private VoidEventChannelSO _executingLoseOnePartSO;
        [SerializeField] private VoidEventChannelSO _executeGainingOnePartSO;


        private void LoseOneSegment()
        {
            int lastSegmentIndex = _wormMechanicHelper.WormDataSO.CurrentWormLength - 1;

            _wormMechanicHelper.UpdateWormAppearance(-1);

            OnWormSegmentDisconnecting();
            _wormMechanicHelper.OnWormLengthValueChangedWrapper(lastSegmentIndex);
        }

        private void OnWormSegmentDisconnecting()
        {
            _wormMechanicHelper.WormSegmentDisconnectingSO.RaiseEvent();
        }

        private void GainOneSegment()
        {
            OnWormSegmentConnecting();

            _wormMechanicHelper.UpdateWormAppearance(1);

            int finalWormLength = _wormMechanicHelper.CalculateWormLengthAfterAttachment(1);
            _wormMechanicHelper.OnWormLengthValueChangedWrapper(finalWormLength);
        }

        private void OnWormSegmentConnecting()
        {
            _wormMechanicHelper.WormSegmentConnectingSO.RaiseEvent();
        }

        private void OnEnable()
        {
            _executingLoseOnePartSO.OnEventRaised += LoseOneSegment;
            _executeGainingOnePartSO.OnEventRaised += GainOneSegment;
        }

        private void OnDisable()
        {
            _executingLoseOnePartSO.OnEventRaised -= LoseOneSegment;
            _executeGainingOnePartSO.OnEventRaised -= GainOneSegment;
        }
    }
}