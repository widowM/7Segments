using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
// Entries to be stored in JSON
public struct LevelEditorEntry
{
    public string poolTag;
    public Point pos;

    public LevelEditorEntry(string pTag, Point p)
    {
        poolTag = pTag;
        pos = p;
    }
}