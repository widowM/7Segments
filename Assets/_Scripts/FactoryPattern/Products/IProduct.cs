using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WormGame.FactoryPattern
{
    /// <summary>
    /// A common interface between products
    /// </summary>
    public interface IProduct
    {
        // Add common properties and methods here
        public string ProductName { get; }
        public GameObject GameObject { get; }

        // Customize this for each concrete product
        public void Initialize();
    }

}