using System.Collections.Generic;
using UnityEngine;
using WormGame.Core.Utils;
using WormGame.EventChannels;
using WormGame.Variables;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace WormGame.Core
{
    ///
    /// <summary>
    /// A central scriptable object that stores the data and some methods of the worm.
    /// It tracks the currently connected segments of the worm in a collection and also holds a collection
    /// for tracking of the disconnected worm chains. It also provides access to the GameplayDataSO, WormStructureDataSO,
    /// WormPhysicsDataSO properties.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/WormGame/Worm/WormDataSO")]
    public class WormDataSO : DescriptionSO
    {
        // Fields    
        private GameObject _player;

        private Vector2 _directionOfMovement;

        [Header("*** WORM DATA ***")]
        [Tooltip("GameplayDataSO data need for gameplay fine tuning")]
        [SerializeField] private GameplayDataSO _gameplayDataSO;
        [SerializeField] private WormCollectionsDataSO _wormCollectionsDataSO;
        [SerializeField] private WormStructureDataSO _wormStructureDataSO;
        [SerializeField] private WormPhysicsDataSO _wormPhysicsDataSO;
        [Header("Weapon Cooldowns")]
        [SerializeField] private float _segmentShootCooldown;
        [SerializeField] private float _biteCooldown;
        [SerializeField] private float _bombCooldown;

        [Tooltip("The worm's head prefab that we are going to instantiate in start of the worm's building.")]
        [SerializeField] private GameObject _wormHeadPrefab;
        [Tooltip("Used for tracking of the current length of the worm measured in worm segments count.")]
        [SerializeField] private int _currentWormLength = 5;
        private GameObject m_Head;
        [SerializeField] private Color _defaultWormColor;
        [SerializeField] private bool _isPowerUpd;
        [Header("Listen to Event Channels")]
        [SerializeField] private IntEventChannelSO _changingWormLengthValue;

        // Properties
        public float SegmentShootCooldown => _segmentShootCooldown;
        public float BiteCooldown => _biteCooldown;
        public float BombCooldown => _bombCooldown;
        public GameObject WormHeadPrefab => _wormHeadPrefab;
        public Color DefaultWormColor => _defaultWormColor;
        public WormCollectionsDataSO WormCollectionsDataSO => _wormCollectionsDataSO;
        public WormStructureDataSO WormStructureDataSO => _wormStructureDataSO;
        public WormPhysicsDataSO WormPhysicsDataSO => _wormPhysicsDataSO;
        public Vector2 DirectionOfMovement => _directionOfMovement;

        public bool IsPowerUpd
        {
            get { return _isPowerUpd; }
            set { _isPowerUpd = value; }
        }


        public GameObject Player
        {
            get { return _player; }
            set 
            {
                if (_player == null)
                {
                    _player = value;
                }
                else
                {
                    Debug.Log("A reference to the Player gameobject in scene is already set!");
                }
            }
        }
        public int CurrentWormLength
        {
            get { return _currentWormLength; }
            set { _currentWormLength = Mathf.Clamp(value, 0, 100); }
        }
        public GameObject Head
        {
            get { return m_Head; }
            set 
            {
                m_Head = value;
            }
        }

        public void InitializeCooldowns()
        {
            _segmentShootCooldown = 1.5f;
            _biteCooldown = 3;
            _bombCooldown = 7; 
        }

        public void DecreaseCooldowns()
        {
            _segmentShootCooldown = _segmentShootCooldown * 0.85f;
            _biteCooldown = _biteCooldown * 0.8f;
            _bombCooldown = _bombCooldown * 0.8f;

        }

        // Methods
        public void InitializeWorm()
        {
            _currentWormLength = _wormStructureDataSO.InitialNumberOfWormSegments;
        }

        public void SetCurrentWormLength(int length)
        {
            _currentWormLength = length;
        }

        public void ResetWormReferences()
        {
            m_Head = null;
            _player = null;
        }

        private void OnEnable()
        {
            _changingWormLengthValue.OnEventRaised += SetCurrentWormLength;
        }

        private void OnDisable()
        {
            _changingWormLengthValue.OnEventRaised -= SetCurrentWormLength;
        }
    }
}