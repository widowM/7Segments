using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WormGame
{
    public class ColorAnimator : MonoBehaviour
    {
        // Fields
        [SerializeField] private Color _defaultColor;
        private Color _currentColor;
        [SerializeField] private SpriteRenderer _spriteRend;

        // Properties
        public Color DefaultColor
        {
            get => _defaultColor;
            set => _defaultColor = value;
        }

        // Methods
        private void Start()
        {
            _currentColor = _defaultColor;
        }

        public void AnimateColorAlpha(float fadeMultiplier)
        {
            _currentColor.a = _defaultColor.a * fadeMultiplier;
            _spriteRend.color = _currentColor;
        }

        public void ResetColor()
        {
            _spriteRend.color = _defaultColor;
            _currentColor = _defaultColor;
        }
    }
}
