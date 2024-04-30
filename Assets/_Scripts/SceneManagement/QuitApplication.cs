using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.EventChannels;

namespace WormGame
{
    public class QuitApplication : MonoBehaviour
    {
        [Header("Broadcast on Event Channels")]
        [SerializeField] private VoidEventChannelSO _applicationQuitSO;

        public void QuitGame()
        {
            _applicationQuitSO.RaiseEvent();
        }
    }
}
