using System;
using UnityEngine;
using WormGame.FactoryPattern;
using WormGame.EventChannels;
using WormGame.Core.Utils;
using TMPro;

namespace WormGame.Core
{
    // Used to disconnect chain starting from the specific alive worm segment hit
    public class WormSegmentDefault : WormSegmentPlayableAbstract
    {
        private int _indexInList;

        public int IndexInList
        {
            get { return _indexInList; }
            set { _indexInList = value; }
        }

        [Header("Broadcast on Event Channels")]
        [SerializeField] private IntEventChannelSO _executeDetachmentFromIndexSO;

        public override void TakeDamage(DamageInfo damageInfo)
        {
            WormStateManager.Instance.GetState().TakeDamage(this, damageInfo);         
        }

        private void OnWormSegmentIndexInListFound(int index)
        {
            _executeDetachmentFromIndexSO.RaiseEvent(index);
        }

        public override void ProcessDamage(DamageInfo damageInfo)
        {
            base.TakeDamage(damageInfo);

            int currentWormLength = _wormDataSO.CurrentWormLength;
            int damage = currentWormLength / 3;
            int newLength = currentWormLength - damage;

            GameObject dmgPopUp = ObjectPooler.Instance.GetObject("DamagePopUpTag", transform.position, Quaternion.identity);
            dmgPopUp.GetComponentInChildren<TextMeshProUGUI>().text = damage.ToString();

            int index = newLength;

            if (index > 0)
            {
                OnWormSegmentIndexInListFound(index);

                GameLog.LogMessage("Index was found, it was: " + index.ToString());
            }
            else
            {
                GameLog.LogMessage("Index was not found!");
            }
        }

        protected override void OnDisable()
        {
            GetComponent<HingeJoint2D>().connectedBody = null;
            base.OnDisable();
        }
    }
}