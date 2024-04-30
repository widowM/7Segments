using UnityEngine;
using WormGame.Variables;
using WormGame.Core;
using WormGame.Core.Utils;
using System;

namespace WormGame.FactoryPattern
{
    /// <summary>
    /// This class inherits from Factory and creates worm stare lines products.
    /// </summary>
    /// <remarks>
    /// This class uses the Factory Method design pattern to encapsulate the logic 
    /// of creating worm stare lines. It overrides the GetProduct method
    /// of the base class and returns an instance of a worm stare line product subclass.
    /// </remarks>
    /// 
    public class WormStareLineConcreteFactory : Factory
    {
        public override IProduct GetProduct<T>(params Vector2[] args)
        {
            // create a Prefab instance and get the product component
            GameObject segment = ObjectPooler.Instance.GetObject(_objectPoolTagsContainerSO.WormStareLineTag);
            if (segment == null)
            {
                // handle the null case
                throw new NullReferenceException("Worm segment's value is null!");
            }
            IProduct wormStareLineProduct = segment.GetComponent<WormStareLineProduct>();

            LineRenderer lineRenderer = segment.GetComponentInChildren<LineRenderer>();

            // Start position
            lineRenderer.SetPosition(0, args[0]);

            // End position
            lineRenderer.SetPosition(1, args[1]);

            // each product contains its own logic
            wormStareLineProduct.Initialize();
            GameLog.LogMessage(GetLog(wormStareLineProduct));

            return wormStareLineProduct;
        }
    }
}
