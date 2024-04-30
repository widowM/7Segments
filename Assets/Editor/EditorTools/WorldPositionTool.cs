#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;
using WormGame.Core.Utils;

[EditorTool("[ Click Position Snapper Tool ]")]
class WorldPositionTool : EditorTool
{

    Vector2 m_WorldPosition;

    // This is called when the tool is activated/deactivated by the editor
    public override void OnActivated()
    {
        SceneView.duringSceneGui += DuringSceneViewGUI;
    }

    public override void OnWillBeDeactivated()
    {
        SceneView.duringSceneGui -= DuringSceneViewGUI;
    }

    void DuringSceneViewGUI(SceneView sceneView)
    {
        // Draw scene view overlay only if this is the active tool
        if (ToolManager.IsActiveTool(this))
        {
            DrawSceneViewOverlay();
            SceneInputAndDrawing(sceneView);
        }
    }

    void DrawSceneViewOverlay()
    {
        const int MARGIN = 6;
        Rect rect = new Rect(MARGIN, MARGIN, 100, 100);
        
        Texture2D tex = new Texture2D(1, 1);
        // Create the color that you want to change to
        Color grey = Color.grey;
        Color g = new Color(grey.r, grey.g, grey.b, 0.5f);

        // Loop over all the pixels in the texture
        for (int y = 0; y < tex.height; y++)
        {
            for (int x = 0; x < tex.width; x++)
            {
                // Set the pixel color to the new color
                tex.SetPixel(x, y, g);
            }
        }

        // Apply the changes to the texture
        tex.Apply();
        // button on top of the scene view to select prefabs
        Handles.BeginGUI();

        DrawTitle(rect, tex);
        DrawHowToLabel(rect, g, tex);
        DrawOutcomeLabel(rect, tex);
        DrawButton(rect);
              
        Handles.EndGUI();
    }

    void DrawTitle(Rect rect, Texture2D tex)
    {
        // Draw title
        GUIStyle guiStyle = new GUIStyle(GUI.skin.label);

        guiStyle.fixedHeight = rect.height * 0.7f;
        guiStyle.fixedWidth =rect.width * 2f;
        guiStyle.fontSize = 20;
        guiStyle.fontStyle = FontStyle.Bold;
        guiStyle.wordWrap = true;
        guiStyle.normal.background = tex;
        guiStyle.alignment = TextAnchor.MiddleCenter;
        guiStyle.normal.background = tex;

        GUILayout.Label("Click Position Snapper Tool", guiStyle);
    }

    void DrawHowToLabel(Rect rect, Color g, Texture2D tex)
    {
        // Draw HOW-TO label
        GUIStyle guiStyle = new GUIStyle(GUI.skin.label);
        guiStyle.fixedHeight = rect.height;
        guiStyle.fixedWidth = rect.width * 2f;
        guiStyle.fontStyle = FontStyle.Italic;
        guiStyle.wordWrap = true;
        guiStyle.fontSize = 11;
        guiStyle.richText = true;
        g = new Color(g.r, g.g, g.b, 0.3f);

        // Loop over all the pixels in the texture
        for (int y = 0; y < tex.height; y++)
        {
            for (int x = 0; x < tex.width; x++)
            {
                // Set the pixel color to the new color
                tex.SetPixel(x, y, g);
            }
        }

        // Apply the changes to the texture
        tex.Apply();

        guiStyle.normal.background = tex;

        GUILayout.Label("<b>HOW TO:</b> 1) Right click anywhere in scene view to copy" +
            " world position to the clipboard and then 2) select" +
            " any gameobject in the hierarchy. Finally, 3) press the 'Snap' button to" +
            " snap the gameobject to the saved position.", guiStyle);
    }

    void DrawOutcomeLabel(Rect rect, Texture2D tex)
    {
        // Draw outcome label
        GUIStyle guiStyle = new GUIStyle(GUI.skin.label);

        guiStyle.fixedHeight = rect.height;
        guiStyle.fixedWidth = rect.width * 2;
        guiStyle.fontSize = 12;
        guiStyle.wordWrap = true;
        guiStyle.richText = true;
        guiStyle.normal.background = tex;

        GUILayout.Label($"The clicked position is <size=14><b>{m_WorldPosition}</b></size> and it" +
            $" is copied to the clipboard. You can <size=14><b>Snap</b></size> the selected gameobject" +
            $" in the hierarchy to the clicked position using the corresponding button.", guiStyle);
    }

    void DrawButton(Rect rect)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(5);

        GUIStyle guiStyle = new GUIStyle(GUI.skin.button);
        guiStyle.fixedHeight = rect.height;
        guiStyle.fixedWidth = rect.width * 2;
        guiStyle.fontSize = 25;
        guiStyle.fontStyle = FontStyle.Bold;
        guiStyle.wordWrap = true;
        guiStyle.normal.textColor = Color.green;
        guiStyle.hover.textColor = Color.green;

        if (GUILayout.Button("Snap to Position", guiStyle))
        {
            if (Selection.activeGameObject != null)
            {
                GameObject go = Selection.activeGameObject;

                Undo.RecordObject(go.transform, "Snap position");
                go.transform.position = m_WorldPosition;
                EditorUtility.SetDirty(go.transform);
                ClipboardHelper.CopyVector2(m_WorldPosition);
            }
        }
        GUILayout.EndHorizontal();
    }

    void SceneInputAndDrawing(SceneView sceneView)
    {
        if (Event.current.type == EventType.MouseMove) 
        {
            sceneView.Repaint();
        }

        // Get the current event
        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 1)
        {
            // Get the world position of the hit point
            m_WorldPosition = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;
            ClipboardHelper.CopyVector2(m_WorldPosition);
        }
    }
}
#endif