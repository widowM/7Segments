using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Variables;
using WormGame.Core;
using WormGame.Core.Utils;
using System;

namespace WormGame.FactoryPattern
{
    /// <summary>
    /// This class inherits from Factory and creates worm segment products.
    /// </summary>
    /// <remarks>
    /// This class uses the Factory Method design pattern to encapsulate the logic 
    /// of creating segment product. It overrides the GetProduct method
    /// of the base class and returns an instance of a worm segment product subclass.
    /// </remarks>
    /// 
    public class WormSegmentConcreteFactory : Factory
    {
        public override IProduct GetProduct<T>(params Vector2[] args)
        {
            // create a Prefab instance and get the product component
            if (ObjectPooler.Instance == null)
            {
                Debug.Log("Object Pooler Values Holder Instance is NULL.");
            }

            GameObject wormSegment = ObjectPooler.Instance.GetObject(_objectPoolTagsContainerSO.WormSegmentDefaultTag);

            if (wormSegment == null)
            {
                // handle the null case
                throw new NullReferenceException("Worm segment's value is null!");
            }

            IProduct wormSegmentProduct = wormSegment.GetComponent<WormDefaultSegmentProduct>();

            // Set position and rotation
            wormSegment.transform.SetPositionAndRotation(args[0], Quaternion.identity);

            // each product contains its own logic
            wormSegmentProduct.Initialize();
            GameLog.LogMessage(GetLog(wormSegmentProduct));

            return wormSegmentProduct;
        }
    }
}