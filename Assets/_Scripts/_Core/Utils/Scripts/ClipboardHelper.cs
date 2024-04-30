using System;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace WormGame.Core.Utils
{
    public static class ClipboardHelper
    {
        // This method copies a Vector2 value to the system clipboard as a string
        public static void CopyVector2(Vector2 vector)
        {
            // Convert the vector to a string using the ToString method
            string vectorString = vector.ToString();

            // Use the GUIUtility.systemCopyBuffer property to set the clipboard value
            GUIUtility.systemCopyBuffer = vectorString;
            // Value was copied
            Debug.Log($"Value {vectorString} is copied to the clipboard");
        }
    }
}