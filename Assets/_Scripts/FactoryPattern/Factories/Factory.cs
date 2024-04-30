using UnityEngine;
using WormGame.Core.Containers;

namespace WormGame.FactoryPattern
{
    /// <summary>
    /// Base class for factories
    /// </summary>
    public abstract class Factory : MonoBehaviour
    {
        [SerializeField] protected ObjectPoolTagsContainerSO _objectPoolTagsContainerSO;
        public abstract IProduct GetProduct<T>(params Vector2[] args);

        // shared method with all factories
        public string GetLog(IProduct product)
        {
            string logMessage = "Factory: created product " + product.ProductName;
            return logMessage;
        }
    }
}