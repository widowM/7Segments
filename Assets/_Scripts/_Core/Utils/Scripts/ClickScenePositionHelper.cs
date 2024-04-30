using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WormGame.Core.Utils
{
    public class ClickScenePositionHelper : MonoBehaviour
    {
        public void ShowPosition(Vector2 pos)
        {
            // Print the world position to the debug log
            Debug.Log("World position: " + pos.ToString());
        }
    }
}