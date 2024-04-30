using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WormGame.EventChannels;

public class ReloadGame : MonoBehaviour
{
    [Header("Broadcast on Event Channels")]
    [SerializeField] private VoidEventChannelSO _selectInitialSceneSO;
    public void StartAgainFromZero()
    {
        _selectInitialSceneSO.RaiseEvent();
    }
}
