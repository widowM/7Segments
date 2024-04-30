using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.FactoryPattern;

namespace WormGame.FactoryPattern
{
    public class RedAppleProduct : MonoBehaviour, IProduct
    {
        [SerializeField] protected string _productName = "Red Apple Product";
        public string ProductName { get => _productName; set => _productName = value; }
        public GameObject GameObject => gameObject;

        public void Initialize()
        {
            // Any unique logic to this product
            gameObject.name = _productName;
        }
    }
}