using UnityEngine;

namespace WormGame.FactoryPattern
{
    public class WormDefaultSegmentProduct : MonoBehaviour, IProduct
    {
        [SerializeField] private WormSegmentDataSO _wormDefaultSegmentDataSO;
        public string ProductName { get => _wormDefaultSegmentDataSO.SegmentName; }
        public GameObject GameObject => gameObject;

        public void Initialize()
        {
            gameObject.name = _wormDefaultSegmentDataSO.SegmentName;
            GetComponentInChildren<SpriteRenderer>().color = _wormDefaultSegmentDataSO.Color;
        }
    }
}