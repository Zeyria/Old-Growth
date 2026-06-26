using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapClass
{
    public enum Enum
    {
        MapSizeSmall = 6,
        MapSizeMedium = 8,
        MapSizeBig = 10,
    };
    public Enum MapSize;
    public MapClass(Enum MapSize)
    {
        this.MapSize = MapSize;
    }
}
