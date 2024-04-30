using UnityEngine;

namespace WormGame.EventChannels
{

    /// <summary>
    /// This is a ScriptableObject-based event that takes an integer as a payload.
    /// </summary> 
    [CreateAssetMenu(menuName = "ScriptableObjects/Events/IntEventChannelSO", fileName = "IntEventChannelSO")]
    public class IntEventChannelSO : GenericEventChannelSO<int>
    {
    }
}