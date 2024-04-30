using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.EventChannels;

namespace WormGame.Core
{
    // Worm is Normal State or Invincible
    public class WormStateManager : MonoBehaviour
    {
        public static WormStateManager Instance { get; private set; }

        private IWormState _currentState;

        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            _currentState = new IWormNormalState();
        }

        public void SetState(IWormState newState)
        {
            _currentState = newState;
        }

        public IWormState GetState()
        {
            return _currentState;
        }
    }
}