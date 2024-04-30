using UnityEngine;
using WormGame.Variables;
using WormGame.Core;
using WormGame.Core.Utils;
using System;
using WormGame.Core.Containers;

namespace WormGame.FactoryPattern
{
    /// <summary>
    /// This class inherits from Factory and creates enemy products.
    /// </summary>
    /// <remarks>
    /// This class uses the Factory Method design pattern to encapsulate the logic 
    /// of creating enemies. It overrides the GetProduct method
    /// of the base class and returns an instance of an Enemy product subclass.
    /// </remarks>
    /// 
    public class EnemyConcreteFactory : Factory
    {
        public override IProduct GetProduct<T>(params Vector2[] args)
        {
            // create a Prefab instance and get the product component
            GameObject enemy = ObjectPooler.Instance.GetObject(_objectPoolTagsContainerSO.EnemyDefaultTag);
            if (enemy == null)
            {
                // handle the null case
                throw new NullReferenceException("Worm segment's value is null!");
            }
            IProduct enemyProduct = enemy.GetComponent<EnemyProduct>();

            // Set position and rotation
            enemy.transform.SetPositionAndRotation(args[0], Quaternion.identity);

            // each product contains its own logic
            enemyProduct.Initialize();
            GameLog.LogMessage(GetLog(enemyProduct));
            return enemyProduct;
        }
    }
}