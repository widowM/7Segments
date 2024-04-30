using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WormGame.Core;

namespace WormGame
{
    [CustomEditor(typeof(WormPhysicsDataSO))]
    public class WormStructureSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector
            base.OnInspectorGUI();

            // Get a reference to the object pooler script
            WormPhysicsDataSO wormPhysicsDataSO = (WormPhysicsDataSO)target;

            // Draw a button to clear the worm collections
            if (GUILayout.Button("Tweak worm physics"))
            {
                OnTweakingWormPhysics(wormPhysicsDataSO);
                
            }
        }

        private void OnTweakingWormPhysics(WormPhysicsDataSO wormPhysicsDataSO)
        {
            wormPhysicsDataSO.TweakingWormPhysics.RaiseEvent();
        }
    }
}
