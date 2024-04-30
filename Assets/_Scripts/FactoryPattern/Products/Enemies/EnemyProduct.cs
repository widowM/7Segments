using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WormGame.FactoryPattern
{
    public class EnemyProduct : MonoBehaviour, IProduct
    {
        [SerializeField] protected string _productName = "Enemy Product";
        public string ProductName { get => _productName; set => _productName = value; }
        public GameObject GameObject => gameObject;

        public void Initialize()
        {
            // Any unique logic to this product
            gameObject.name = _productName;
        }
    }

}