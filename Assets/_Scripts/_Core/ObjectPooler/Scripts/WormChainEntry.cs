using System.Collections.Generic;
using System;

/// <summary>
/// Worm chain store format
/// </summary>
[Serializable]
public struct WormChainEntry
{
    public int length;
    public List<Point> points;

    public WormChainEntry(int chainLength, List<Point> segmentPositions)
    {
        length = chainLength;
        points = segmentPositions;
    }
}

[Serializable]
public struct Point
{
    public float x;
    public float y;

    public Point(float xPos, float yPos)
    {
        x = xPos;
        y = yPos;
    }
}