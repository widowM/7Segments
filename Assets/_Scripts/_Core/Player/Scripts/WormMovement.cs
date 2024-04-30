using UnityEngine;
using WormGame.FactoryPattern;
using WormGame.EventChannels;
using WormGame.Managers;
using UnityEngine.Android;

namespace WormGame.Core.Player
{
    /// <summary>
    /// This class is responsible for controlling the movement and rotation of the worm head.
    /// It uses the worm input and data to calculate the direction and angle of the head towards
    /// the mouse position. It also uses raycasting to detect if the head is looking at any body
    /// parts and spawn a stare line if so. It uses a factory to create the stare line product
    /// and an object pooler to reuse it.
    /// </summary>
    public class WormMovement : MonoBehaviour, IUpdatable
    {
        [SerializeField] private Worm _worm;
        private Rigidbody2D _rb2D;
        private Vector2 _headPosition;
        private Vector2 _mouseWorldPosition;
        private Vector2 _headLookDirection;
        private bool _headLookingAtBodyParts = false;
        private bool _spawnedLine = false;
        private float _angleOfRotation;

        public Vector2 HeadLookDirection => _headLookDirection;
        private RaycastHit2D[] _results = new RaycastHit2D[5];
        [SerializeField] private WormStareLineConcreteFactory _stareLineFactory;

        [SerializeField] private LayerMask _layerNotToLookAt;
        [Header("Listen to Event Channels")]
        [SerializeField] private VoidEventChannelSO _gameStartedSO;
        private void Start()
        {
            GameObjectRB2D headSegment = _worm.WormCollectionsDataSO.ConnectedSegments[0];
            _rb2D = headSegment.Rb2D;
        }

        public void UpdateMe()
        {
            _headPosition = _worm.WormDataSO.Head.transform.position;
            _mouseWorldPosition = _worm.WormInput.MouseWorldPosition;

            _headLookDirection = GetHeadLookDirection(_mouseWorldPosition, _headPosition);
         
            RunStareLineLogic();

            _angleOfRotation = GetAngleOfRotation();
        }

        private void FixedUpdate()
        {
            if (_headLookingAtBodyParts)
                return;

            if (IsCursorDistancedFromHead())
            {
                RotateHead();

                SetHeadVelocity();
            }
        }

        private void RunStareLineLogic()
        {
            int bodyHitRayCount = Physics2D.RaycastNonAlloc(_headPosition,
                _headLookDirection,
                _results,
                Mathf.Infinity,
                _layerNotToLookAt);

            if (bodyHitRayCount == 0)
            {
                _headLookingAtBodyParts = false;
                _spawnedLine = false;
            }

            else
            {
                _headLookingAtBodyParts = true;
                Vector2 endPos = _results[0].transform.position;

                Vector2 startPos = _worm.WormDataSO.Head.transform.position;
                SpawnStareLine(startPos, endPos);
                _spawnedLine = true;
            }
        }

        private void RotateHead()
        {     
            Quaternion headRotation = _rb2D.transform.rotation;
            float headRotationSpeed = _worm.WormPhysicsDataSO.WormHeadRotationSpeed;

            // Rotate the head segment towards the target angle
            _rb2D.MoveRotation(Quaternion.Lerp(headRotation,
                TargetRotation(in _angleOfRotation),
                headRotationSpeed * Time.fixedDeltaTime));
        }

        bool IsCursorDistancedFromHead()
        {
            return Vector2.Distance(_worm.WormDataSO.Head.transform.position, _worm.WormInput.MouseWorldPosition) > 3;
        }
        private void SetHeadVelocity()
        {
            float headMovementSpeed = _worm.WormPhysicsDataSO.WormMovementSpeed;

            // Set the velocity of the head rigidbody to the direction vector multiplied by the speed
            _rb2D.velocity = _headLookDirection * headMovementSpeed;
        }

        private float GetAngleOfRotation()
        {
            Vector2 mousePos;
            Vector2 mouseWorldPos = _worm.WormInput.MouseWorldPosition;
            Transform head = _worm.WormDataSO.Head.transform;
            Vector2 headPos = head.position;
            mousePos.x = mouseWorldPos.x - headPos.x;
            mousePos.y = mouseWorldPos.y - headPos.y;

            return Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        }

        private Quaternion TargetRotation(in float angle)
        {
            return Quaternion.Euler(new Vector3(0, 0, angle));
        }

        private void SpawnStareLine(Vector2 startPosition, Vector2 endPosition)
        {
            if (_spawnedLine)
                return;

            Vector2[] startEndPositions = { startPosition, endPosition };
            _stareLineFactory.GetProduct<IProduct>(startEndPositions);
        }

        private Vector2 GetHeadLookDirection(in Vector2 mouseWorldPos, in Vector2 headPosition)
        {
            return (mouseWorldPos - headPosition).normalized;
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