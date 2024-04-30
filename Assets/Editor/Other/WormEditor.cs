#if UNITY_EDITOR
using DG.DOTweenEditor;
using UnityEditor;
using UnityEngine;
using WormGame.Core.Player;

namespace WormGame.EditorTools
{
    [CustomEditor(typeof(Worm))]
    public class WormEditor : Editor
    {
        private Editor _editorInstance;
        private GUIStyle _labelStyle;

        private void OnEnable()
        {
            // reset the editor instance
            _editorInstance = null;

            _labelStyle = Core.Utils.EditorUtils.CreateDefaultLabelStyle();
        }

        public override void OnInspectorGUI()
        {
            // the inspected target component
            Worm worm = (Worm)target;
            if (_editorInstance == null)
                _editorInstance = Editor.CreateEditor(worm.WormDataSO);

            // Draw the custom label
            Core.Utils.EditorUtils.DrawCustomLabel("Worm Manager", _labelStyle);

            // show the variables from the MonoBehaviour
            base.OnInspectorGUI();

            // draw the ScriptableObjects inspector
            _editorInstance.DrawDefaultInspector();         
        }
    }
}
#endif