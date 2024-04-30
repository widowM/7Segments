#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using WormGame.Core;

namespace WormGame
{
    [CustomEditor(typeof(WormDataSO))]
    public class WormDataSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector
            base.OnInspectorGUI();

            // Get a reference to the object pooler script
            WormDataSO wormDataSO = (WormDataSO)target;

            // Draw a button to clear the worm collections
            if (GUILayout.Button("Clear Collections"))
            {
                // Call a method to clear the collections
                //Debug.Log("Button action is not set.");
                wormDataSO.WormCollectionsDataSO.ClearWormSegmentsCollection();
                wormDataSO.WormCollectionsDataSO.ClearWormChainsCollections();
            }
        }
    }
}
#endif