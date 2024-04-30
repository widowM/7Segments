using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WormGame/Worm/WormSegmentDataSO")]
public class WormSegmentDataSO : ScriptableObject
{
    [SerializeField] private string _segmentName;
    [SerializeField] private Color _color;

    public string SegmentName => _segmentName;
    public Color Color => _color;
}