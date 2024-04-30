using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WormGame.Core.Enemies
{
    /// <summary>
    /// This class keeps shared enemy type data.
    /// (flyweight pattern)
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/WormGame/EnemySO")]
    public class EnemySO : ScriptableObject
    {
        [SerializeField] private Color _defaultColor;
        [SerializeField] private float _totalHp = 3;
        [SerializeField] private float _knockbackAmount;
        [SerializeField] private AudioClip _impactSfx;
        [SerializeField] private float _impactSfxVolume;

        public Color DefaultColor
        {
            get { return _defaultColor; }
            set { _defaultColor = value; }
        }
        public float TotalHp
        {
            get { return _totalHp; }
        }

        public float KnockbackAmount
        {
            get { return _knockbackAmount; }
        }

        public AudioClip ImpactSfx
        {
            get { return _impactSfx; }
        }

        public float ImpactSfxVolume
        {
            get { return _impactSfxVolume; }
        }
    }
}