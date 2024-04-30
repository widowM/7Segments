using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Core;
using WormGame.Managers;

namespace WormGame.Environment
{
    public class BackgroundMovement : MonoBehaviour, IUpdatable
    {
        [SerializeField] private WormDataSO _wormDataSO;
        [SerializeField] private float _moveSpeed = 0.2f;

        public void UpdateMe()
        {
            MoveTowardsPlayerMovementDirection();
        }

        private void MoveTowardsPlayerMovementDirection()
        {
            Vector2 dir = _wormDataSO.DirectionOfMovement.normalized;

            transform.position = Vector3.MoveTowards(transform.position, dir, _moveSpeed * Time.deltaTime);
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
