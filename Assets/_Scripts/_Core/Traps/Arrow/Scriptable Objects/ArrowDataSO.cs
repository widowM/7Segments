using System.Collections.Generic;
using UnityEngine;
using WormGame.Spawn;

namespace WormGame.Core.Traps
{
    /// <summary> This class inherits from ScriptableObject and stores the data for different types of arrows.
    /// // It has the following fields: // - _arrowType: a string that indicates the name and category of the arrow,
    /// such as “Red Arrow” or “Blue Arrow”. // - _arrowSpeed: a float that represents the speed of the arrow when fired,
    /// - _arrowColor: a Color that defines the tint of the arrow sprite, such as red or blue. //
    /// - arrowSpawnAreas: a list of SpawnAreaSO that specifies the possible spawning locations of the arrows on the scene.
    /// </summary>

    [CreateAssetMenu(menuName = "ScriptableObjects/WormGame/ArrowData")]
    public class ArrowDataSO : ScriptableObject
    {
        [SerializeField] private string _arrowType;
        [SerializeField] private float _arrowSpeed = 6;
        [SerializeField] private Color _arrowColor;
        [SerializeField] private List<SpawnAreaSO> _arrowSpawnAreasSO;

        public string ArrowType => _arrowType;
        public float ArrowSpeed => _arrowSpeed;
        public Color ArrowColor => _arrowColor;
        public List<SpawnAreaSO> ArrowSpawnAreas => _arrowSpawnAreasSO;
    }
}