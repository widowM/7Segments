using System.Collections.Generic;
using UnityEngine;
using WormGame.Variables;

namespace WormGame.Core.Containers
{
    [CreateAssetMenu(menuName = "ScriptableObjects/WormGame/Containers/ObjectPoolTagsContainerSO")]
    public class ObjectPoolTagsContainerSO : ScriptableObject
    {
        // Fields
        [Header("Object Pool tag of each poolable gameobject")]
        [Header("Worm Segments")]
        [SerializeField] private StringVariableSO _wormSegmentDefaultTagSO;
        [SerializeField] private StringVariableSO _wormSegmentConnectableTagSO;
        [SerializeField] private StringVariableSO _wormSegmentDisconnectedTagSO;
        [Header("Worm Features")]
        [SerializeField] private StringVariableSO _wormProjectileTagSO;
        [SerializeField] private StringVariableSO _wormStareLineTagSO;
        [Header("Enemies")]
        [SerializeField] private StringVariableSO _enemyDefaultTagSO;
        [Header("Environmental Traps")]
        [SerializeField] private StringVariableSO _redArrowTagSO;
        [Header("Particle Effects")]
        [SerializeField] private StringVariableSO _impactParticleTagSO;
        [SerializeField] private List<StringVariableSO> _bloodSplatTagsSOs;
        [Header("Power Ups")]
        [SerializeField] private StringVariableSO _redAppleTagSO;
        [SerializeField] private StringVariableSO _goldenAppleTagSO;


        // Properties
        public string WormSegmentDefaultTag => _wormSegmentDefaultTagSO.Value;
        public string WormSegmentConnectableTag => _wormSegmentConnectableTagSO.Value;
        public string WormSegmentDisconnectedTag => _wormSegmentDisconnectedTagSO.Value;
        public string WormProjectileTag => _wormProjectileTagSO.Value;
        public string WormStareLineTag => _wormStareLineTagSO.Value;
        public string EnemyDefaultTag => _enemyDefaultTagSO.Value;
        public string RedArrowTag => _redArrowTagSO.Value;
        public string ImpactParticleTag => _impactParticleTagSO.Value;
        public List<StringVariableSO> BloodSplatTags => _bloodSplatTagsSOs;
        public string RedAppleTagSO => _redAppleTagSO.Value;
        public string GoldenAppleTagSO => _goldenAppleTagSO.Value;
    }
}

