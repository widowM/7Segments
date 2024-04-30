using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Variables;
using WormGame.Core;
using WormGame.Core.Utils;
using System;
using WormGame.Core.Containers;


namespace WormGame.FactoryPattern
{
    /// <summary>
    /// This class inherits from Factory and creates worm connectable segment products.
    /// </summary>
    /// <remarks>
    /// This class uses the Factory Method design pattern to encapsulate the logic 
    /// of creating connectable segments. It overrides the GetProduct method
    /// of the base class and returns an instance of a connectable segment product subclass.
    /// </remarks>
    /// 
    public class WormConnectableSegmentConcreteFactory : Factory
    {
        public override IProduct GetProduct<T>(params Vector2[] args)
        {
            // create a Prefab instance and get the product component
            GameObject segment = ObjectPooler.Instance.GetObject(_objectPoolTagsContainerSO.WormSegmentConnectableTag);

            if (segment == null)
            {
                // handle the null case
                throw new NullReferenceException("Segment's value is null!");
            }
            IProduct wormConnectableSegmentProduct = segment.GetComponent<WormConnectableSegmentProduct>();

            // each product contains its own logic
            wormConnectableSegmentProduct.Initialize();
            GameLog.LogMessage(GetLog(wormConnectableSegmentProduct));

            return wormConnectableSegmentProduct;
        }
    }

}