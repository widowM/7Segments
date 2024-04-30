using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.EventChannels;

namespace WormGame.Core
{
    /// <summary>
    /// Structure of worm and details about segment spacing, initial number of connected worm segments etc.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/WormGame/Worm/WormStructureData", fileName = "WormStructureData")]
    public class WormStructureDataSO : ScriptableObject
    {
        [Header("Worm structure")]
        //
        [Tooltip("The initial number of worm segments")]
        [Range(1, 16)]
        [SerializeField] private int _initialNumberOfWormSegments = 8;
        [Tooltip("(HingeJoint2D) Offset for subsequent worm segments' placement in the chain. We need" +
            " this for both initialization and segments' (re)attachment")]
        [Range(0.1f, 2.5f)]
        [SerializeField] private float _segmentOffsetXPos;

        [Tooltip("(HingeJoint2D) The space (in units) between the subsequent worm segments")]
        [Range(0.1f, 2)]
        [SerializeField] private float _segmentSpacing;


        // Properties
        public int InitialNumberOfWormSegments
        {
            get { return _initialNumberOfWormSegments; }
            set { _initialNumberOfWormSegments = Mathf.Clamp(value, 1, 16); }
        }
        public float SegmentOffsetXPos => _segmentOffsetXPos;
        public float SegmentSpacing => _segmentSpacing;

        public float GetAnchor()
        {
            return _segmentSpacing / 2;
        }
    }
}