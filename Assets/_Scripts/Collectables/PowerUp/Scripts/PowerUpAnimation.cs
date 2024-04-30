using UnityEngine;
using DG.Tweening;

namespace WormGame.Collectables
{
    public class PowerUpAnimation : MonoBehaviour
    {
        // Fields
        [SerializeField] private PowerUp _powerUp;
        [SerializeField] private SpriteRenderer _powerupSpriteRend;
        [SerializeField] private Ease _animationEase;
        private Tween _powerUpTween;

        // Methods
        private void OnEnable()
        {
            float minDuration = _powerUp.PowerUpSO.AnimationDurationMin;
            float maxDuration = _powerUp.PowerUpSO.AnimationDurationMax;
            float animationDuration = Random.Range(minDuration, maxDuration);

            _powerUpTween = transform.DORotate(new Vector3(0, 360, 0), animationDuration, RotateMode.WorldAxisAdd).
                SetLoops(-1, LoopType.Yoyo).SetEase(_animationEase);
        }

        private void OnDisable()
        {
            _powerUpTween.Kill();
        }
    }
}