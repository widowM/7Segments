using UnityEngine;

namespace WormGame.FactoryPattern
{
    /// <summary> This class implements IProduct and creates the worm disconnected segment that can only be part of
    /// a disconnected chain of segments. It uses the product name and the game object to identify and access the segment.
    /// It also defines an Initialize method that sets the name of the game object to the product name.
    /// </summary>
    public class WormDisconnectedSegmentProduct : MonoBehaviour, IProduct
    {
        [SerializeField] private WormSegmentDataSO _disconnectedSegmentDataSO;
        public string ProductName { get => _disconnectedSegmentDataSO.SegmentName;  }
        public GameObject GameObject => gameObject;

        public void Initialize()
        {
            gameObject.name = _disconnectedSegmentDataSO.name;
        }

        private void OnDisable()
        {
            GetComponentInChildren<SpriteRenderer>().color = _disconnectedSegmentDataSO.Color;
            GetComponent<HingeJoint2D>().connectedBody = null;
        }

        private void OnEnable()
        {
            GetComponentInChildren<SpriteRenderer>().color = _disconnectedSegmentDataSO.Color;
            GetComponent<HingeJoint2D>().connectedBody = null;
        }
    }
}