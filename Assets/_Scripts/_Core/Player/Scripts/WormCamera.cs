using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.EventChannels;

namespace WormGame.Core.Player
{
    public class WormCamera : MonoBehaviour
    {
        [SerializeField] private Worm _worm;
        [SerializeField] private CinemachineVirtualCamera _cinemachineCam;

        [Header("Broadcast on Event Channels")]
        [SerializeField] private VoidEventChannelSO _cinemachineCameraReady;
        [Header("Listen to Event Channels")]
        [SerializeField] private VoidEventChannelSO _headReferencedSO;

        private void OnEnable()
        {
            _headReferencedSO.OnEventRaised += SetCinemachineFollowTarget;
        }

        private void OnDisable()
        {
            _headReferencedSO.OnEventRaised -= SetCinemachineFollowTarget;
        }

        private void SetCinemachineFollowTarget()
        {
            _cinemachineCam.Follow = _worm.WormDataSO.Head.transform;
            _cinemachineCameraReady.RaiseEvent();
        }
    }
}