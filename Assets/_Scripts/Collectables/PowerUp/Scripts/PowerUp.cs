using UnityEngine;
using WormGame.Core;
using WormGame.EventChannels;

namespace WormGame.Collectables
{
    /// <summary>
    /// This class represents a power up that can be interacted with by the player.
    /// It implements the IInteractable interface and its method Interact(GameObject go) method.
    /// It passes the gameobject that interacted with this power up as a parameter and applies effects to it.
    /// It also notifies any listeners about the interaction.
    /// It returns the power up to its pool using the ReturnToPool() method.
    /// </summary>
    public  class PowerUp : MonoBehaviour, IInteractable
    {
        // Fields
        [SerializeField] protected PowerUpSO _powerUpSO;
        [SerializeField] protected PowerUpEventChannelSO _powerUpEventSO;

        // Properties
        public PowerUpSO PowerUpSO => _powerUpSO;
        // Methods
        public void Interact(in GameObject head)
        {
            _powerUpEventSO.RaiseEvent(_powerUpSO);

            ReturnToPool();
        }

        private void ReturnToPool()
        {
            string tag = GetComponent<PoolNameHolder>().PoolName.Value;
            ObjectPooler.Instance.ReturnObject(gameObject, tag);
        }
    }
}