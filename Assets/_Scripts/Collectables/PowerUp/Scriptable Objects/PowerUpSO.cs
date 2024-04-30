using UnityEngine;
using WormGame.EventChannels;

namespace WormGame.Collectables
{
    /// <summary>
    /// This abstract class inherits from ScriptableObject and represents
    /// a power up that can be applied to a gameobject. It has an abstract method
    /// called Apply(GameObject) that takes a gameobject as a parameter and applies
    /// the power up effects to it. The effects may vary depending on the type and properties of the power up,
    /// such as increasing speed, health, damage, etc.
    /// </summary>

    [CreateAssetMenu(menuName = "ScriptableObjects/WormGame/PowerUpSO", fileName = "PowerUpSO")]
    public abstract class PowerUpSO : ScriptableObject
    {
        // Fields
        [SerializeField] protected float _animationDurationMin = 0.3f;
        [SerializeField] protected float _animationDurationMax = 0.5f;
        [SerializeField] protected Sprite _icon;
        [SerializeField] protected string _powerUpName;
        [SerializeField] protected Color _color;
        [SerializeField] protected AudioClip _pickupClip;
        [Header("Broadcast on Event Channels")]
        [SerializeField] protected VoidEventChannelSO _appliedPowerUpSO;

        // Properties
        public float AnimationDurationMin => _animationDurationMin;
        public float AnimationDurationMax => _animationDurationMax;
        public Sprite Icon => _icon;
        public Color Color => _color;
        public AudioClip PickupClip => _pickupClip;
        public string PowerUpName => _powerUpName;

        // Methods
        public abstract void Apply();
    }
}
