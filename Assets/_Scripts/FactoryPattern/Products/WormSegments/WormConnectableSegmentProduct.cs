using UnityEngine;

namespace WormGame.FactoryPattern
{
    /// <summary> /// This class implements IProduct and creates the worm connectable segment
    /// that can be used to connect the chain to the worm. It also implements the IInteractable interface
    /// to handle the interaction of the connectable segment with the worm head and the IProduct interface
    /// and implements its method Initialize().
    /// </summary>
    public class WormConnectableSegmentProduct : MonoBehaviour, IProduct
    {
        [SerializeField] private WormSegmentDataSO _wormConnectableSegDataSO;

        public string ProductName { get => _wormConnectableSegDataSO.SegmentName; }

        public GameObject GameObject => gameObject;

        public void Initialize()
        {
            gameObject.name = _wormConnectableSegDataSO.SegmentName;
        }
    }
}