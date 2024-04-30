using System.Collections.Generic;
using UnityEngine;
using WormGame.EventChannels;

namespace WormGame.Core.Player
{
    /// <summary>
    /// This class is responsible for implementing the core mechanic of the worm character,
    /// which is the ability to attach and detach segments of its body. It has references to several event channels
    /// that communicate with other classes and scripts, such the WormChainGenerator and WormMechanicHelper. It has the following methods:
    /// - StartInvincibilityCountdown: This method starts a coroutine that makes the worm invincible for a given number of seconds.
    /// It also sends the appropriate messages to play and stop the invincibility animation using the event channels.
    /// - ReturnDisconnectedChainObjectsToPool: This method takes a game object as a parameter, representing the connectable segment key of
    /// the disconnected chain. It uses the ObjectPooler to return the chain objects to their respective pools, based on their tags.
    /// - RemoveAttachedChainFromDictionary: This method takes a game object as a parameter, representing the connectable segment key
    /// of the attached chain. It removes the chain from the dictionary of the worm's data.
    /// </summary>
    public class WormAttachDetachChainMechanic : MonoBehaviour
    {
        [SerializeField] private WormMechanicHelper _wormMechanicHelper;

        [Header("Broadcast on Event Channels")]
        [SerializeField] private IntVector2EventChannelSO _generatingDetachedChainSO;
        [SerializeField] private VoidEventChannelSO _gotInvincibilitySO;
        [SerializeField] private VoidEventChannelSO _wormHitSO;
        [SerializeField] private FloatEventChannelSO _pauseTimeForSecondsSO;

        [Header("Listen to Event Channels")]
        [SerializeField] private IntEventChannelSO _executeDetachmentFromIndexSO;
        [SerializeField] private IntGameObjectEventChannelSO _executingAttachmentOfChainSO;
        [SerializeField] private VoidEventChannelSO _gotDamagedSO;

        private void DetachChain(int detachmentIndex)
        {
            OnDetached();
            OnWormHit();
            OnGoingInvincible();

            // The change in length of worm. In case of detachment we are going to need a negative value
            int change = -(_wormMechanicHelper.WormDataSO.CurrentWormLength - detachmentIndex);
            _wormMechanicHelper.UpdateWormAppearance(change);

            int currentLength = detachmentIndex;
            _wormMechanicHelper.OnWormLengthValueChangedWrapper(currentLength);
            float pauseTimeDuration = 0.3f;
            OnPausingTimeForSeconds(pauseTimeDuration);
        }

        private void OnPausingTimeForSeconds(float pauseDuration)
        {
            _pauseTimeForSecondsSO.RaiseEvent(pauseDuration);
        }

        private void OnGoingInvincible()
        {
            _gotInvincibilitySO.RaiseEvent();
        }
        private int CalculateDetachedChainLength(in int detachmentIndex)
        {
            int wormLength = _wormMechanicHelper.WormCollectionsDataSO.ConnectedSegments.Count;

            return wormLength - detachmentIndex;
        }

        private Vector2 GetDetachmentSegmentPosition(in int detachmentIndex)
        {
            GameObject segment = _wormMechanicHelper.WormCollectionsDataSO.ConnectedSegments[detachmentIndex].Go;

            return segment.transform.position;
        }

        private void OnDetached()
        {
            _wormMechanicHelper.WormSegmentDisconnectingSO.RaiseEvent();        
        }

        private void OnWormHit()
        {
            _wormHitSO.RaiseEvent();
        }

        private void OnGeneratingDetachedChain(int lengthOfChain, Vector2 connectableSegmentPos)
        {
            _generatingDetachedChainSO.RaiseEvent(lengthOfChain, connectableSegmentPos);
        }

        private void AttachChain(int lengthOfChainOfSegments, GameObject connectableSegment)
        {
            _wormMechanicHelper.OnAttachedWrapper();
            int finalWormLength = _wormMechanicHelper.CalculateWormLengthAfterAttachment(in lengthOfChainOfSegments);
            ReturnDisconnectedChainObjectsToPool(in connectableSegment);
            RemoveAttachedChainFromDictionary(connectableSegment);

            // The difference between previous and next length of worm. In the case of attachment we need a positive value
            int change = _wormMechanicHelper.WormDataSO.CurrentWormLength + lengthOfChainOfSegments;
            _wormMechanicHelper.UpdateWormAppearance(change);

            _wormMechanicHelper.OnWormLengthValueChangedWrapper(finalWormLength);

            OnPausingTimeForSeconds(0.3f);
        }

        private void ReturnDisconnectedChainObjectsToPool(in GameObject connectableSegmentKey)
        {
            ReturnConnectableSegment(connectableSegmentKey);
            List<GameObject> chain = _wormMechanicHelper.WormCollectionsDataSO.
                WormDisconnectedChainsOfSegments[connectableSegmentKey];

            for (int i = 1; i < chain.Count; i++)
            {
                ReturnDisconnectedSegment(chain[i]);
            }
        }

        private void ReturnConnectableSegment(in GameObject connectableSegmentKey)
        {
            GameObject connectableSegment = 
                _wormMechanicHelper.WormCollectionsDataSO.WormDisconnectedChainsOfSegments[connectableSegmentKey][0];
            ObjectPooler.Instance.
                ReturnObject(in connectableSegment, _wormMechanicHelper.ObjectPoolTagsContainerSO.WormSegmentConnectableTag);
        }

        private void ReturnDisconnectedSegment(in GameObject disconnectedSegment)
        {
            ObjectPooler.Instance.
                ReturnObject(in disconnectedSegment, _wormMechanicHelper.ObjectPoolTagsContainerSO.WormSegmentDisconnectedTag);
        }

        private void RemoveAttachedChainFromDictionary(in GameObject connectableSegmentKey)
        {
            _wormMechanicHelper.WormCollectionsDataSO.WormDisconnectedChainsOfSegments.Remove(connectableSegmentKey);
        }

        private void OnEnable()
        {
            _executeDetachmentFromIndexSO.OnEventRaised += DetachChain;
            _executingAttachmentOfChainSO.OnEventRaised += AttachChain;
        }

        private void OnDisable()
        {
            _executeDetachmentFromIndexSO.OnEventRaised -= DetachChain;
            _executingAttachmentOfChainSO.OnEventRaised -= AttachChain;
        }
    }
}