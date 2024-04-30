using UnityEngine;
using UnityEngine.Events;
using WormGame.Core.Utils;

namespace WormGame.EventChannels
{
    public abstract class GenericEventChannelWithTwoParametersSO<T0, T1> : DescriptionSO
    {
        [Tooltip("The action to perform; Listeners subscribe to this UnityAction")]
        public UnityAction<T0, T1> OnEventRaised;

        public void RaiseEvent(T0 parameter0, T1 parameter1)
        {

            if (OnEventRaised == null)
                return;

            OnEventRaised.Invoke(parameter0, parameter1);
        }
    }

    // To create additional event channels, simply derive a class from GenericEventChannelWithTwoParametersSO
    // filling in the types T0, T1. Leave the concrete implementation blank. This is a quick way
    // to create new event channels.

    // For example:
    //[CreateAssetMenu(menuName = "Events/FloatInt EventChannel", fileName = "FloatIntEventChannel")]
    //public class
}