using UnityEngine;

namespace WormGame.EventChannels
{
    /// <summary>
    /// A Scriptable Object-based event that passes an int and a gameobject as a payload.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/Events/IntGameObjectEventChannelSO", fileName = "IntGameObject")]
    public class IntGameObjectEventChannelSO : GenericEventChannelWithTwoParametersSO<int, GameObject>
    {
    }
}