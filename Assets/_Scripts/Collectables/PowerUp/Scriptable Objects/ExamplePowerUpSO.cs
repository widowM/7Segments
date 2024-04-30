using UnityEngine;

namespace WormGame.Collectables
{
    [CreateAssetMenu(menuName = "ScriptableObjects/WormGame/PowerUp/")]
    public class ExamplePowerUpSO : PowerUpSO
    {
        // Methods
        public override void Apply()
        {
            _appliedPowerUpSO.RaiseEvent();
        }
    }
}