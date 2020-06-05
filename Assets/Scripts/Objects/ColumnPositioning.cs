using System;
using UnityEngine;

[Serializable]
public class ColumnPositioning
{
    [SerializeField]
    public int columns;
    [SerializeField]
    public float InitialXPos;
    [SerializeField]
    public float InitialYPos;
    [SerializeField]
    public float xOffset;
    [SerializeField]
    public float yOffset;

    public Vector3 CalculatePosition(int itemIndex)
    {
        var pos = new Vector3(InitialXPos, InitialYPos, 0);

        pos.x += xOffset * (itemIndex % columns);
        pos.y -= yOffset * (itemIndex / columns);

        return pos;
    }
}