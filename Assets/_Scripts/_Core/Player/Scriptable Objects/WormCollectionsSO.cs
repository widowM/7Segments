using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Collectables;

namespace WormGame.Core
{
    [CreateAssetMenu(menuName = "ScriptableObjects/WormGame/Worm/WormCollectionsDataSO")]
    public class WormCollectionsDataSO : ScriptableObject
    {
        [SerializeField] private string _playerID;
        // We are storing both worm body segments gameobjects and rigidbodies because we are
        // going to need both for a) part rigidbodies movement b) hinge jointed connection of
        // consecutive rigidbodies c) removal and insertion of worm body parts gameobjects
        private List<GameObjectRB2D> _connectedSegments = new List<GameObjectRB2D>();

        // This dictionary is going to store the connectable worm segment game object as a key and
        // a list of the disconnected parts gameobjects as a value for easier reattachment of worm segments body
        // parts to the main (controlled) body
        private Dictionary<GameObject, List<GameObject>> _wormDisconnectedChainsOfSegments =
            new Dictionary<GameObject, List<GameObject>>();
        [SerializeField] private List<PowerUpSO> _powerUpSOs = new List<PowerUpSO>();

        // Properties
        public List<GameObjectRB2D> ConnectedSegments
        {
            get { return _connectedSegments; }
            set { _connectedSegments = value; }    
        }
        public Dictionary<GameObject, List<GameObject>> WormDisconnectedChainsOfSegments =>
            _wormDisconnectedChainsOfSegments;

        public List<PowerUpSO> PowerUpSOs => _powerUpSOs;

        // Methods
        public void ClearWormSegmentsCollection()
        {
            _connectedSegments.Clear();
            _connectedSegments = new List<GameObjectRB2D> { };
        }

        public void ClearWormChainsCollections()
        {
            _wormDisconnectedChainsOfSegments.Clear();
            Debug.Log("Cleared dictionary of chains");
        }

        public void ClearPowerUpsSOsCollection()
        {
            _powerUpSOs.Clear();
        }

    }

    [System.Serializable]
    public struct GameObjectRB2D
    {
        private GameObject _go;
        private Rigidbody2D _rb2D;

        public GameObject Go
        {
            get { return _go; }
            set { _go =  value; }
        }

        public Rigidbody2D Rb2D
        { 
            get { return _rb2D; }
            set { _rb2D = value; }
        }

        public GameObjectRB2D(GameObject g, Rigidbody2D r)
        {
            _go = g;
            _rb2D = r;
        }
    }
}