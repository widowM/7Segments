using System;
using UnityEngine;
using WormGame.Core;
using WormGame.Core.Utils;
using WormGame.Core.Containers;

namespace WormGame.FactoryPattern
{
    /// <summary>
    /// This class inherits from Factory and creates red arrow products.
    /// </summary>
    /// <remarks>
    /// This class uses the Factory Method design pattern to encapsulate the logic 
    /// of creating red arrows. It overrides the GetProduct method
    /// of the base class and returns an instance of Red Arrow product subclass.
    /// </remarks>
    /// 
    public class RedArrowConcreteFactory : Factory
    {
        public override IProduct GetProduct<T>(params Vector2[] args)
        {
            string redArrowPoolTag = _objectPoolTagsContainerSO.RedArrowTag;

            // create a Prefab instance and get the product component
            GameObject instance = ObjectPooler.Instance.GetObject(redArrowPoolTag);
            if (instance == null)
            {
                // handle the null case
                throw new NullReferenceException("Red arrow instance value is null!");
            }

            IProduct redArrowProduct = instance.GetComponent<RedArrowProduct>();

            // each product contains its own logic
            redArrowProduct.Initialize();
            GameLog.LogMessage(GetLog(redArrowProduct));

            return redArrowProduct;
        }
    }
}
