using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootGameUtil
{
    public static bool IsNearBy(Vector2 moushPos, Vector2 chessPoint, float threshold)
    {
        if (Math.Abs(moushPos.x - chessPoint.x) + Math.Abs(moushPos.y - chessPoint.y) < threshold)
        {
            return true;
        }

        return false;
    }
}
