using UnityEngine;
using WormGame.EventChannels;

namespace WormGame.UI
{
    public class ButtonSceneLoader : MonoBehaviour
    {
        // Added this flag to ensure that the scene loader is not called twice
        private bool _notifiedFlag = false;
        // Inspector fields
        [SerializeField] private VoidEventChannelSO _selectInitialSceneSO;

        public void NotifyTheSceneLoader()
        {
            if (_notifiedFlag)
                return;
            _notifiedFlag = true;
            _selectInitialSceneSO.RaiseEvent();
        }
    }
}
