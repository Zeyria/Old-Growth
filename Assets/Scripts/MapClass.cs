using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapClass
{
    public enum Enum
    {
        MapSizeSmall = 5,
        MapSizeMedium = 7,
        MapSizeBig = 9,
    };
    public Enum MapSize;
    public MapClass(Enum MapSize)
    {
        this.MapSize = MapSize;
    }
}
