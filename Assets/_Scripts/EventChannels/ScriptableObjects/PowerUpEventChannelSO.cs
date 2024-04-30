using UnityEngine;
using WormGame.Collectables;

namespace WormGame.EventChannels
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Events/PowerUpEventChannelSO", fileName = "PowerUpEventChannel")]
    public class PowerUpEventChannelSO : GenericEventChannelSO<PowerUpSO>
    {
        
    }
}