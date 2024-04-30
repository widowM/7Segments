using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WormGame.FactoryPattern
{
    public class DamagePopupProduct : MonoBehaviour, IProduct
    {
        [SerializeField] protected string _productName = "Damage Popup Product";
        public string ProductName { get => _productName; set => _productName = value; }
        public GameObject GameObject => gameObject;

        public void Initialize()
        {
            // Any unique logic to this product
            gameObject.name = _productName;
        }
    }
}

