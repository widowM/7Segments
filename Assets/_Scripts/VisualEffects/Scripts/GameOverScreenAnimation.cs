using UnityEngine;
using DG.Tweening;

namespace WormGame.VisualEffects
{

    /// <summary>
    /// This class is responsible for animating the game over screen using the DOTween library.
    /// It uses the OnEnable method to scale the transform up and down repeatedly using the DOScale method.
    /// </summary>
    public class GameOverScreenAnimation : MonoBehaviour
    {
        [SerializeField] private float _maxScale = 1.4f;
        [SerializeField] private float _animationCycleDurationInSeconds = 3;

        private void AnimateGameOverScreen()
        {
            transform.DOScale(
                new Vector2(_maxScale, _maxScale/2),
                _animationCycleDurationInSeconds
                ).SetLoops(-1, LoopType.Yoyo);
        }

        private void OnEnable()
        {
            AnimateGameOverScreen();
        }
    }
}