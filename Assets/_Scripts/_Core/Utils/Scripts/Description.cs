using UnityEngine;

namespace WormGame.Core.Utils
{

    /// <summary>
    /// This is a simple component for holding notes or explanation.
    /// </summary>
    public class Description : MonoBehaviour
    {
        [TextArea(5, 25)]
        public string note;

    }
}