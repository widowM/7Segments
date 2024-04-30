using System;
using UnityEngine;
using WormGame.VisualEffects;
using WormGame.Core;
using WormGame.Core.Utils;
using WormGame.Core.Containers;

namespace WormGame.FactoryPattern
{
    /// <summary>
    /// This class inherits from Factory and creates random blood splat products.
    /// </summary>
    /// <remarks>
    /// This class uses the Factory Method design pattern to encapsulate the logic 
    /// of creating different types of blood splat products. It overrides the GetProduct method
    /// of the base class and returns a random instance of a blood splat product subclass.
    /// </remarks>
    /// 
    public class BloodSplatConcreteFactory : Factory
    {
        public override IProduct GetProduct<T>(params Vector2[] args)
        {
            int bloodSplatIndex = GetRandomBloodSplatIndex();
            string bloodSplatPoolTag = GetBloodSplatPoolTag(bloodSplatIndex);

            // create a Prefab instance and get the product component
            GameObject bloodSplat = ObjectPooler.Instance.GetObject(bloodSplatPoolTag);

            if (bloodSplat == null )
            {
                // handle the null case
                throw new NullReferenceException("BloodSplat value is null!");
            }

            bloodSplat.transform.position = args[0];

            IProduct bloodSplatProduct = bloodSplat.GetComponent<BloodSplatProduct>();

            // each product contains its own logic
            bloodSplatProduct.Initialize();
            GameLog.LogMessage(GetLog(bloodSplatProduct));

            return bloodSplatProduct;
        }

        private int GetRandomBloodSplatIndex()
        {
            return UnityEngine.Random.Range(0, _objectPoolTagsContainerSO.BloodSplatTags.Count - 1);
        }

        private string GetBloodSplatPoolTag(int index)
        {
            return _objectPoolTagsContainerSO.BloodSplatTags[index].Value;
        }
    }
}
