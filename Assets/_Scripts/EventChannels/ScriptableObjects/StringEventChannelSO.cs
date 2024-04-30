using UnityEngine;

namespace WormGame.EventChannels
{
    /// <summary>
    /// General event channel that broadcasts and carries string payload.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/Events/StringEventChannelSO", fileName = "StringEventChannel")]
    public class StringEventChannelSO : GenericEventChannelSO<string>
    {
    }

}