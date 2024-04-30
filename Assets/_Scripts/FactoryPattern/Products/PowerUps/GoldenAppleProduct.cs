using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WormGame.FactoryPattern;

public class GoldenAppleProduct : MonoBehaviour, IProduct
{
    [SerializeField] protected string _productName = "Golden Apple Product";
    public string ProductName { get => _productName; set => _productName = value; }
    public GameObject GameObject => gameObject;

    public void Initialize()
    {
        // Any unique logic to this product
        gameObject.name = _productName;
    }
}
