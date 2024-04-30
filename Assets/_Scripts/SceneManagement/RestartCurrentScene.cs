using UnityEngine;
using WormGame.EventChannels;

namespace WormGame
{
    public class RestartCurrentScene : MonoBehaviour
    {
        [Header("Broadcast on Event Channels")]
        [SerializeField] private VoidEventChannelSO _resetSceneSO;
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _resetSceneSO.RaiseEvent();
            }
        }
    }
}
