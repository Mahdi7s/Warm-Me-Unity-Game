using UnityEngine;
using System.Collections;

public class MathUtility
{
    public static float XDiff(float x1, float x2)
    {
        return Mathf.Abs(x1 < x2 ? x2 - x1 : x1 - x2);
    }
}
