using UnityEngine;
using WormGame.EventChannels;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using WormGame.Variables;

namespace WormGame.Core.Player
{
    /// <summary>
    /// Worm Input Handler class
    /// </summary>
    public class WormInputActions : MonoBehaviour
    {
        [SerializeField] private Worm _worm;
        private Transform _projectileSegmentContainer;
        private bool _isCooldown = false;
        [SerializeField] private StringVariableSO _bombTagSO;
        [SerializeField] private AudioClip _emptyAmmoClip;
        private delegate void AttackDelegate();
        private AttackDelegate currentAttack;
        private AttackDelegate bitingAttack;
        private AttackDelegate shootingAttack;
        private AttackDelegate bombingAttack;

        [Header("Broadcast on Event Channels")]
        [SerializeField] private VoidEventChannelSO _executingLoseOnePartSO;
        [SerializeField] private VoidEventChannelSO _attackingSO;
        [SerializeField] private VoidEventChannelSO _bitingStartedSO;
        [SerializeField] private VoidEventChannelSO _applyLastPowerUp;
        [SerializeField] private StringEventChannelSO _attackUINameChangedSO;
        [SerializeField] private VoidEventChannelSO _switchingWeaponSO;
        [SerializeField] private IntEventChannelSO _showWeaponVisualsSO;
        [SerializeField] private FloatEventChannelSO _passCooldownValueSO;

        [Header("Listen to Event Channels")]
        [SerializeField] private VoidEventChannelSO _firstActionButtonPressedSO;
        [SerializeField] private VoidEventChannelSO _secondActionButtonPressedSO;
        [SerializeField] private VoidEventChannelSO _attackMeterFilled;
        [SerializeField] private IntEventChannelSO _passNewWeaponIdEventSO;
        [SerializeField] private VoidEventChannelSO _asyncLoadCompletedSO;

        private void Awake()
        {
            bitingAttack = BitingAttack;
            shootingAttack = ShootingAttack;
            bombingAttack = BombingAttack;
        }

        private void Initialize()
        {
            Transform head = _worm.WormDataSO.Head.transform;
            _projectileSegmentContainer = head.GetChild(0);
            int randomWeaponID = Random.Range(0, 3);
            Debug.Log("Chose weapon id " + randomWeaponID);
            SwitchWeapon(randomWeaponID);
        }

        private void ExecuteFirstAction()
        {
            if (_isCooldown)
                return;
            if (_worm.WormDataSO.WormCollectionsDataSO.ConnectedSegments.Count < 2)
                return;

            currentAttack?.Invoke();
        }

        private void SwitchWeapon(int weaponId)
        {
            switch (weaponId)
            {
                case 0:
                    currentAttack = shootingAttack;
                    _attackUINameChangedSO.RaiseEvent("shoot");
                    _passCooldownValueSO.RaiseEvent(_worm.WormDataSO.SegmentShootCooldown);
                    _showWeaponVisualsSO.RaiseEvent(0);
                    Debug.Log("shoot");
                    break;
                case 1:
                    currentAttack = bitingAttack;
                    _attackUINameChangedSO.RaiseEvent("bite");
                    _passCooldownValueSO.RaiseEvent(_worm.WormDataSO.BiteCooldown);
                    _showWeaponVisualsSO.RaiseEvent(1);
                    Debug.Log("bite");
                    break;
                case 2:
                    currentAttack = bombingAttack;
                    _attackUINameChangedSO.RaiseEvent("bomb");
                    _passCooldownValueSO.RaiseEvent(_worm.WormDataSO.BombCooldown);
                    _showWeaponVisualsSO.RaiseEvent(2);
                    Debug.Log("bomb");
                    break;
                default:
                    Debug.Log("Wrong weapon id.");
                    break;
            }
        }

        private void SwitchWeaponAndPlaySound(int weaponId)
        {
            SwitchWeapon(weaponId);
            OnSwitchingWeapon();
        }

        private void OnSwitchingWeapon()
        {
            _switchingWeaponSO.RaiseEvent();
        }

        private void ShootingAttack()
        {
            int aliveSegmentsCount = _worm.WormDataSO.CurrentWormLength;

            if (aliveSegmentsCount < 2)
            {
                AudioSource.PlayClipAtPoint(_emptyAmmoClip, _worm.WormDataSO.Head.transform.position, 1);
                return;
            }

            OnExecutingLoseOneSegment();
            GameObject wormProjectile = ObjectPooler.Instance
                .GetObject(_worm.ObjectPoolTagsContainerSO.WormProjectileTag);

            if (wormProjectile != null)
            {
                SetInitialProjectilePosition(wormProjectile);
                SetInitialProjectileVelocity(wormProjectile);
            }

            OnAttacking();
        }

        private void BitingAttack()
        {
            int aliveSegmentsCount = _worm.WormDataSO.CurrentWormLength;
            if (aliveSegmentsCount < 2)
            {
                AudioSource.PlayClipAtPoint(_emptyAmmoClip, _worm.WormDataSO.Head.transform.position, 1);
                return;
            }
            OnAttacking();
            OnExecutingLoseOneSegment();
            OnBitingStarted();
            // The below buggy line of unwanted behaviour makes it funny but only works if worm movement is not velocity based
            //_worm.WormDataSO.Head.GetComponent<Rigidbody2D>()
            //    .AddForce(_worm.WormMovement.HeadLookDirection * 200, ForceMode2D.Impulse);
        }

        private void BombingAttack()
        {
            
            List<GameObjectRB2D> connectedSegments = _worm.WormDataSO.WormCollectionsDataSO.ConnectedSegments;
            int aliveSegmentsCount = _worm.WormDataSO.CurrentWormLength;
            if (aliveSegmentsCount < 2)
            {
                AudioSource.PlayClipAtPoint(_emptyAmmoClip, _worm.WormDataSO.Head.transform.position, 1);
                return;
            }
            OnAttacking();
            OnExecutingLoseOneSegment();
            Vector2 lastSegmentPos = connectedSegments[connectedSegments.Count - 1].Go.transform.position;
            GameObject bomb = ObjectPooler.Instance.GetObject(_bombTagSO.Value, lastSegmentPos, Quaternion.identity);
            bomb.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        
        private void OnAttacking()
        {
            _attackingSO.RaiseEvent();

            _isCooldown = true;
        }

        private void OnBitingStarted()
        {
            _bitingStartedSO.RaiseEvent();
        }
        private void CanShootAgain()
        {
            _isCooldown = false;
        }

        private void OnExecutingLoseOneSegment()
        {
            _executingLoseOnePartSO.RaiseEvent();
        }

        private void ExecuteSecondAction()
        {
            _applyLastPowerUp.RaiseEvent();
        }

        private void SetInitialProjectilePosition(GameObject wormProjectile)
        {
            wormProjectile.transform.position = _projectileSegmentContainer.position;
        }

        private void SetInitialProjectileVelocity(in GameObject wormProjectile)
        {
            Rigidbody2D wormProjectileRb = wormProjectile.GetComponent<Rigidbody2D>();
            float force = _worm.WormDataSO.WormPhysicsDataSO.ShootingForce;
            wormProjectileRb.velocity = _worm.WormDataSO.Head.transform.right * force;
        }

        private void OnEnable()
        {
            _firstActionButtonPressedSO.OnEventRaised += ExecuteFirstAction;
            _secondActionButtonPressedSO.OnEventRaised += ExecuteSecondAction;
            _attackMeterFilled.OnEventRaised += CanShootAgain;
            _passNewWeaponIdEventSO.OnEventRaised += SwitchWeaponAndPlaySound;
            _asyncLoadCompletedSO.OnEventRaised += Initialize;
        }

        private void OnDisable()
        {
            _firstActionButtonPressedSO.OnEventRaised -= ExecuteFirstAction;
            _secondActionButtonPressedSO.OnEventRaised -= ExecuteSecondAction;
            _attackMeterFilled.OnEventRaised -= CanShootAgain;
            _passNewWeaponIdEventSO.OnEventRaised -= SwitchWeaponAndPlaySound;
            _asyncLoadCompletedSO.OnEventRaised -= Initialize;
        }
    }
}