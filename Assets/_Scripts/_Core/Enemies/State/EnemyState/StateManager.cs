using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Core;
using WormGame.Managers;

namespace WormGame.Core.Enemies
{
    public class StateManager : MonoBehaviour, IUpdatable
    {
        [SerializeField] private EnemyState _currentState; 

        public void UpdateMe()
        {
            RunStateMachine();
        }

        private void RunStateMachine()
        {
            EnemyState nextState = _currentState?.RunCurrentState();

            if (nextState != null)
            {
                SwitchToTheNextState(nextState);
            }
        }

        private void SwitchToTheNextState(EnemyState nextState)
        {
            _currentState = nextState;
        }

        private void OnEnable()
        {
            UpdateManager.Instance.Register(this);
        }

        private void OnDisable()
        {
            UpdateManager.Instance.Unregister(this);
        }
    }
}