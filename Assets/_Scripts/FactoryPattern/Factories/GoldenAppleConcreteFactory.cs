using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Core.Containers;
using WormGame.Core.Utils;
using WormGame.Core;
using WormGame.FactoryPattern;

namespace WormGame.FactoryPattern
{
    public class GoldenAppleConcreteFactory : Factory
    {
        public override IProduct GetProduct<T>(params Vector2[] args)
        {
            // create a Prefab instance and get the product component
            GameObject gApple = ObjectPooler.Instance.GetObject(_objectPoolTagsContainerSO.GoldenAppleTagSO);
            if (gApple == null)
            {
                // handle the null case
                throw new NullReferenceException("Golden Apple's value is null!");
            }
            IProduct gAppleProduct = gApple.GetComponent<GoldenAppleProduct>();

            // Set position and rotation
            gApple.transform.SetPositionAndRotation(args[0], Quaternion.identity);

            // each product contains its own logic
            gAppleProduct.Initialize();
            GameLog.LogMessage(GetLog(gAppleProduct));
            return gAppleProduct;
        }
    }
}
