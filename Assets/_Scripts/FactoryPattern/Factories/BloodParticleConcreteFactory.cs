using System;
using UnityEngine;
using WormGame.Core.Utils;
using WormGame.Core;
using WormGame.FactoryPattern;
using WormGame.VisualEffects;

namespace WormGame.FactoryPattern
{
    public class BloodParticleConcreteFactory : Factory
    {
        public override IProduct GetProduct<T>(params Vector2[] args)
        {
            string impactParticlePoolTag = _objectPoolTagsContainerSO.ImpactParticleTag;

            // create a Prefab instance and get the product component
            GameObject instance = ObjectPooler.Instance.GetObject(impactParticlePoolTag, args[0], Quaternion.identity);
            if (instance == null)
            {
                // handle the null case
                throw new NullReferenceException("Impact particle instance value is null!");
            }

            IProduct impactParticleProduct = instance.GetComponent<ImpactParticleProduct>();

            // each product contains its own logic
            impactParticleProduct.Initialize();
            GameLog.LogMessage(GetLog(impactParticleProduct));

            return impactParticleProduct;
        }

        public void SpawnParticle(Vector2 spawnPos)
        {
            Vector2[] spawnP = { spawnPos };
            GetProduct<IProduct>(spawnP);
        }
    }
}