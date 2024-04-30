using UnityEngine;

namespace WormGame.EventChannels
{
    /// <summary>
    /// A Scriptable Object-based event that passes two floats as a payload.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/Events/FloatFloatEventChannelSO", fileName = "FloatFloatEventSO")]
    public class FloatFloatEventChannelSO : GenericEventChannelWithTwoParametersSO<float, float>
    {
    }

}