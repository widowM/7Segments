using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WormGame.Core.Utils
{
    // Utility class for the editor tools
    public static class EditorUtils
    {
        public static void SetTexturePixelsToColor(Texture2D tex, Color color)
        {
            // Loop over all the pixels in the texture
            for (int y = 0; y < tex.height; y++)
            {
                for (int x = 0; x < tex.width; x++)
                {
                    // Set the pixel color to the new color
                    tex.SetPixel(x, y, color);
                }
            }
            tex.Apply();
        }

#if UNITY_EDITOR
        public static Vector2 GetMouseWorldPos()
        {
            return HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
        }

        public static Texture2D CreateBlueTexture()
        {
            int width = 1; 
            int height = 1; 

            Texture2D texture = new Texture2D(width, height);
            Color[] pixels = new Color[width * height];

            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = Color.blue;
            }

            texture.SetPixels(pixels);
            texture.Apply();

            return texture;
        }

        public static GUIStyle CreateDefaultLabelStyle()
        {
            // Define the style for the label
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.normal.textColor = Color.white;
            labelStyle.normal.background = CreateBlueTexture();
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.fontSize = 14;
            labelStyle.fontStyle = FontStyle.Bold;

            return labelStyle;
        }
        public static void DrawCustomLabel(string labelText, GUIStyle labelStyle)
        {
            // Draw the label
            EditorGUILayout.LabelField(labelText, labelStyle);
        }
#endif
        public static void CreatePoolsIfNeeded()
        {
            GameObject pools = GameObject.FindWithTag("POOLS");

            if (pools == null)
            {
                GameObject.Find("Manager_ObjectPooling").GetComponent<ObjectPooler>().CreatePools();
            }
        }
    }
}