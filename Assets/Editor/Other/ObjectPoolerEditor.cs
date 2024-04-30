#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using WormGame.Core;

namespace WormGame
{
    [CustomEditor(typeof(ObjectPooler))]
    public class ObjectPoolerEditor : Editor
    {
        private Editor _editorInstance;
        private GUIStyle _labelStyle;

        public override void OnInspectorGUI()
        {
            // Draw the custom label
            Core.Utils.EditorUtils.DrawCustomLabel("Object Pooler", _labelStyle);

            // Draw the default inspector
            base.OnInspectorGUI();

            // Get a reference to the object pooler script
            ObjectPooler pooler = (ObjectPooler)target;

            // Draw a button to clear the collections
            if (GUILayout.Button("Clear Pools"))
            {
                // Call a method on the object pooler script to clear the collections
                pooler.ClearPools();
            }

            // Draw a button to recreate the collections
            if (GUILayout.Button("Create Pools"))
            {
                // Call a method on the object pooler script to recreate the collections
                pooler.CreatePools();
            }
        }

        private void OnEnable()
        {
            // reset the editor instance
            _editorInstance = null;

            _labelStyle = Core.Utils.EditorUtils.CreateDefaultLabelStyle();
        }

    }
}
#endif