using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Managers;

namespace WormGame.Core.Enemies
{
    public class DiageyEnemy : Enemy//, IUpdatable
    {
        [SerializeField] private Transform _spriteTransform;
        [SerializeField] private Vector2 _currentPosition;
        [SerializeField] private Vector2 _lastPosition;
        private Quaternion _rot;


        // Properties

        public Quaternion Rot => _rot;

        private void Start()
        {
            _lastPosition = transform.position;
        }
        //public void UpdateMe()
        //{
        //    transform.rotation = _spriteTransform.rotation;

        //    Vector3 currentPosition = transform.position;

        //    _lastPosition = currentPosition;
        //}

        //private void OnEnable()
        //{
        //    UpdateManager.Instance.Register(this);
        //}

        //private void OnDisable()
        //{
        //    UpdateManager.Instance.Unregister(this);
        //}
    }

}
