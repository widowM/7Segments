using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.EventChannels;

namespace WormGame.Core
{
    public class WormConnectableSegment : MonoBehaviour, IInteractable
    {
        [SerializeField] private WormCollectionsDataSO _wormCollectionsDataSO;

        [Header("Broadcast on Event Channels")]
        [SerializeField] private IntGameObjectEventChannelSO _executingAttachmentOfChain;

        public void Interact(in GameObject go)
        {
            GameObject connectableSegment = gameObject;
            int lengthOfChain = _wormCollectionsDataSO.WormDisconnectedChainsOfSegments[connectableSegment].Count;

            OnPreparedForAttachment(lengthOfChain, connectableSegment);
        }

        private void OnPreparedForAttachment(int lengthOfChain, GameObject connectableSegment)
        {
            _executingAttachmentOfChain.RaiseEvent(lengthOfChain, connectableSegment);
        }
    }
}