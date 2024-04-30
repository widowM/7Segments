using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Collectables;
using WormGame.Core;
using WormGame.EventChannels;

namespace WormGame
{
    public class Weapon : MonoBehaviour, IInteractable
    {
        // Fields
        [SerializeField] private WeaponSO _weaponSO;
        [Header("Broadcast on Event Channels")]
        [SerializeField] private IntEventChannelSO _passNewWeaponIdEventSO;

        // Properties
        public WeaponSO WeaponSO => _weaponSO;
        // Methods
        public void Interact(in GameObject head)
        {
            _passNewWeaponIdEventSO.RaiseEvent(_weaponSO.Id);

            ReturnToPool();
        }

        private void ReturnToPool()
        {
            string tag = GetComponent<PoolNameHolder>().PoolName.Value;
            ObjectPooler.Instance.ReturnObject(gameObject, tag);
        }
    }
}
