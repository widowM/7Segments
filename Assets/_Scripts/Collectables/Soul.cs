using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Core;
using WormGame.EventChannels;
using WormGame.Variables;

namespace WormGame
{
    public class Soul : MonoBehaviour
    {
        [SerializeField] private StringVariableSO _soulTag;
        [Header("Broadcast on Event Channels")]
        [SerializeField] private VoidEventChannelSO _soulPickedSO;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Head"))
            {
                Debug.Log("Soul Picked");
                _soulPickedSO.RaiseEvent();
                ObjectPooler.Instance.ReturnObject(gameObject, _soulTag.Value);

            }
        }
    }
}
