using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.EventChannels;

/// <summary>
/// A Scriptable Object-based event that passes an int and a gameobject as a payload.
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/Events/IntVector2EventChannelSO", fileName = "IntVector2EventChannelSO")]
public class IntVector2EventChannelSO : GenericEventChannelWithTwoParametersSO<int, Vector2>
{
}