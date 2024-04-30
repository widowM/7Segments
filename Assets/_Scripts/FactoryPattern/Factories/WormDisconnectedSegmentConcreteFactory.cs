using UnityEngine;
using WormGame.Core;
using WormGame.Core.Utils;
using System;
using WormGame.Core.Containers;


namespace WormGame.FactoryPattern
{
    /// <summary>
    /// This class inherits from Factory and creates disconnected segment products.
    /// </summary>
    /// <remarks>
    /// This class uses the Factory Method design pattern to encapsulate the logic 
    /// of creating disconnected segments. It overrides the GetProduct method
    /// of the base class and returns an instance of a disconnected product subclass.
    /// </remarks>
    /// 
    public class WormDisconnectedSegmentConcreteFactory : Factory
    {
        public override IProduct GetProduct<T>(params Vector2[] args)
        {
            // create a Prefab instance and get the product component
            GameObject segment = ObjectPooler.Instance.GetObject(_objectPoolTagsContainerSO.WormSegmentDisconnectedTag, args[0], Quaternion.identity);
            if (segment == null)
            {
                // handle the null case
                throw new NullReferenceException("Segment's value is null!");
            }

            IProduct wormDisconnectedSegmentProduct = segment.GetComponent<WormDisconnectedSegmentProduct>();

            // each product contains its own logic
            wormDisconnectedSegmentProduct.Initialize();
            GameLog.LogMessage(GetLog(wormDisconnectedSegmentProduct));

            return wormDisconnectedSegmentProduct;
        }
    }
}
