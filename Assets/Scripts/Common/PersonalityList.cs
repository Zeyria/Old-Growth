using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StringIntArray
{
    public string stri;
    public int[] intArray;

    public StringIntArray(string stri, int[] intArray)
    {
        this.stri = stri;
        this.intArray = intArray;
    }
}
public class PersonalityList : MonoBehaviour
{
    // HP, Speed, Sight, Attack
    //HP is 2 while all others is 1
    public static int[] Relaxed = { 2, -1, 0, 0 };
    public static int[] Impish = { 2, 0, -1, 0 };
    public static int[] Bold = { 2, 0, 0, -1 };

    public static int[] Jolly = { 0, 1, -1, 0 };
    public static int[] Timid = { 0, 1, 0, -1 };
    public static int[] Hasty = { -2, 1, 0, 0 };

    public static int[] Modest = { 0, 0, 1, -1 };
    public static int[] Mild = { -2, 0, 1, 0 };
    public static int[] Quiet = { 0, -1, 1, 0 };

    public static int[] Adamant = { 0, 0, -1, 1 };
    public static int[] Brave = { 0, -1, 0, 1 };
    public static int[] Lonely = { -2, 0, 0, 1 };

    public static List<StringIntArray> Personalites = new List<StringIntArray> { new StringIntArray( "Relaxed", Relaxed), new StringIntArray("Impish", Impish), new StringIntArray("Bold", Bold), new StringIntArray("Jolly", Jolly), new StringIntArray("Timid", Timid),
        new StringIntArray("Hasty", Hasty), new StringIntArray("Modest", Modest), new StringIntArray("Mild", Mild), new StringIntArray("Quiet", Quiet), new StringIntArray("Adamant", Adamant), new StringIntArray("Brave", Brave), new StringIntArray("Lonely", Lonely), };
}
