using UnityEngine;
using UnityEngine.Events;
using WormGame.Core.Utils;

namespace WormGame.EventChannels
{
    /// <summary>
    /// General Event Channel that carries no extra data.
    /// </summary>

    [CreateAssetMenu(menuName = "ScriptableObjects/Events/VoidEventChannelSO", fileName = "VoidEventChannel")]
    public class VoidEventChannelSO : DescriptionSO
    {
        [Tooltip("The action to perform")]
        public UnityAction OnEventRaised;

        public void RaiseEvent()
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke();
        }
    }
}