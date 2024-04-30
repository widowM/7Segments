using UnityEngine;
using UnityEngine.Events;
using WormGame.Core.Utils;

namespace WormGame.EventChannels
{
    public abstract class GenericEventChannelSO<T> : DescriptionSO
    {
        [Tooltip("The action to perform; Listeners subscribe to this UnityAction")]
        public UnityAction<T> OnEventRaised;

        public void RaiseEvent(T parameter)
        {

            if (OnEventRaised == null)
                return;

            OnEventRaised.Invoke(parameter);

        }
    }

    // To create addition event channels, simply derive a class from GenericEventChannelSO
    // filling in the type T. Leave the concrete implementation blank. This is a quick way
    // to create new event channels.

    // For example:
    //[CreateAssetMenu(menuName = "Events/Float EventChannel", fileName = "FloatEventChannel")]
    //public class
}