using UnityEngine;
using WormGame.Core.Utils;

namespace WormGame.Core
{
    /// <summary>
    /// General GameplayDataSO data and camera shake data for testing.
    /// Holds a WormStructureDataSO and a WormPhysicsDataSO.
    /// The WormStructureDataSO stores the initial number of worm’s segments and segment spacing.
    /// The WormPhysicsDataSO stores the data for the worm’s movement speed, rotation speed, and total
    /// linear and angular drag.
    /// Adjust these to customize the GameplayDataSO.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/WormGame/GameplayDataSO", fileName = "GameplayDataSO")]
    public class GameplayDataSO : DescriptionSO
    {
        [Header("Camera shake tweaking")]

        [Range(0, 30)]
        [SerializeField] private float _camShakeIntensity;

        [Range(0, 1)]
        [SerializeField] private float _cameraShakeDuration;

        // Properties

        public float CameraShakeIntensity
        {
            get { return _camShakeIntensity; }
            set { _camShakeIntensity = Mathf.Clamp(value, 0, 30); }
        }
        public float CameraShakeDuration
        {
            get { return _cameraShakeDuration; }
            set { _cameraShakeDuration = Mathf.Clamp(value, 0, 1); }
        }
    }
}
