using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Core.Utils;
using WormGame.Core;
using WormGame.FactoryPattern;

namespace WormGame.FactoryPattern
{
    public class DamagePopupConcreteFactory : Factory
    {
        public override IProduct GetProduct<T>(params Vector2[] args)
        {
            // create a Prefab instance and get the product component
            GameObject damagePopup = ObjectPooler.Instance.GetObject("DamagePopUpTag", args[0], Quaternion.identity);
            if (damagePopup == null)
            {
                // handle the null case
                throw new NullReferenceException("Worm segment's value is null!");
            }
            IProduct damagePopupProduct = damagePopup.GetComponent<DamagePopupProduct>();

            // each product contains its own logic
            damagePopupProduct.Initialize();
            GameLog.LogMessage(GetLog(damagePopupProduct));
            
            return damagePopupProduct;
        }

        public void SpawnDamagePopup(int damage, Vector2 spawnPos)
        {
            Vector2[] spawnP = { spawnPos };
            GetProduct<IProduct>(spawnP);
        }
    }
}