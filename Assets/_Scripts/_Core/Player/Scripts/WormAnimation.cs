using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.EventChannels;
using WormGame;
using WormGame.Managers;

namespace WormGame.Core.Player
{
    /// <summary>
    /// This class is responsible for animating the worm’s segments using a color transition effect.
    /// It has references to the Worm script and the starting and ending colors of the effect. 
    /// It also has a field for the animation speed and a field for the current color.
    /// It uses the Update method to interpolate the current color between the starting and ending
    /// colors using the Mathf.PingPong function and the Time.time value. It then applies the current
    /// color to the sprite renderer of each segment of the worm using a for loop.
    /// It uses the OnDisable method to reset the color of each segment to the starting color when the class is disabled.
    /// </summary>
    public class WormAnimation : MonoBehaviour, IUpdatable
    {
        [SerializeField] private Worm _worm;
        [SerializeField] private float _animationSpeed = 1;

        [Header("Broadcast on Event Channels")]
        [SerializeField] private VoidEventChannelSO _resettingColorSO;

        public void UpdateMe()
        {
            float currentColorAlphaMultiplier = Mathf.Lerp(1, 0, Mathf.PingPong(Time.time * _animationSpeed, 1));

            foreach (GameObjectRB2D segment in _worm.WormCollectionsDataSO.ConnectedSegments)
            {
                ColorAnimator[] colorAnimators = segment.Go.GetComponentsInChildren<ColorAnimator>();
                foreach (var colorAnimator in colorAnimators)
                {
                    colorAnimator.AnimateColorAlpha(currentColorAlphaMultiplier);
                }
            }
        }

        private void ResetColor()
        {

            foreach (GameObjectRB2D segment in _worm.WormCollectionsDataSO.ConnectedSegments)
            {
                if (segment.Go != null)
                {
                    ColorAnimator[] colorAnimators = segment.Go.GetComponentsInChildren<ColorAnimator>();
                    foreach (var colorAnimator in colorAnimators)
                    {
                        colorAnimator.ResetColor();
                    }
                }
            }
        }

        private void OnEnable()
        {
            UpdateManager.Instance.Register(this);
        }

        private void OnDisable()
        {
            UpdateManager.Instance.Unregister(this);
            ResetColor();
        }
    }
}