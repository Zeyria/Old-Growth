using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComFunc
{
    public static Vector3 GridToWorldSpace(float x, float y)
    {
        return new Vector3((x * -3.5f) + (y * 3.5f), y * 1.75f + (x * 1.75f), 0);
    }
    public static Vector2 WorldToGridSpace(float x, float y)
    {
        return new Vector2((1.75f * x - 3.5f * y) / -12.25f, (-1.75f * x - 3.5f * y) / -12.25f);
    }
    public static Vector3Int GridToTileSpace(int x, int y)
    {
        return new Vector3Int(y * 7, x * 7);
    }
    public static Vector2 TileToGridSpace(int x, int y)
    {
        return new Vector2(y / 7f, x / 7f);
    }

    /// <summary>
    /// This has inherent rounding to int.
    /// </summary>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public static Vector2 WorldToTileSpace(Vector3 vec3)
    {
        Vector2 vec2 = ComFunc.WorldToGridSpace(vec3.x, vec3.y);
        vec2 = new Vector2(Mathf.RoundToInt(vec2.y * 7 - .5f), Mathf.RoundToInt(vec2.x * 7 - .5f));
        return vec2;
    }
}
