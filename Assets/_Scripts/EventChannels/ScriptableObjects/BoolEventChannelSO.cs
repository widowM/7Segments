using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WormGame.EventChannels
{
    /// <summary>
    /// A Scriptable Object-based event that passes a bool as a payload.
    /// </summary>
    [CreateAssetMenu(fileName = "BoolEventChannel", menuName = "ScriptableObjects/Events/BoolEventChannelSO")]
    public class BoolEventChannelSO : GenericEventChannelSO<bool>
    {

    }
}