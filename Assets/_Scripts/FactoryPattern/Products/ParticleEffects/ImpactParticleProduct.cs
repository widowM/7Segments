using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.Core.Utils;

namespace WormGame.FactoryPattern
{
    public class ImpactParticleProduct : MonoBehaviour, IProduct
    {
        [SerializeField] private string _productName = "Arrow Product";

        public string ProductName { get => _productName; set => _productName = value; }
        public GameObject GameObject => gameObject;

        public void Initialize()
        {
            // Any unique logic to this product
            gameObject.name = _productName;
        }
    }
}