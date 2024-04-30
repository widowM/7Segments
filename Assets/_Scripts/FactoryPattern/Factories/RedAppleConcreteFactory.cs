using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Core.Utils;
using WormGame.Core;

namespace WormGame.FactoryPattern
{
    public class RedAppleConcreteFactory : Factory
    {
        public override IProduct GetProduct<T>(params Vector2[] args)
        {
            // create a Prefab instance and get the product component
            GameObject rApple = ObjectPooler.Instance.GetObject(_objectPoolTagsContainerSO.RedAppleTagSO);
            if (rApple == null)
            {
                // handle the null case
                throw new NullReferenceException("Red Apple's value is null!");
            }
            IProduct rAppleProduct = rApple.GetComponent<RedAppleProduct>();

            // Set position and rotation
            rApple.transform.SetPositionAndRotation(args[0], Quaternion.identity);

            // each product contains its own logic
            rAppleProduct.Initialize();
            GameLog.LogMessage(GetLog(rAppleProduct));
            return rAppleProduct;
        }
    }
}
