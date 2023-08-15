using System;
using UnityEngine;

public static class ClearBrick
{
    public static void Execute(NumberedBrick[,] field, Vector2Int coords, Action onComplete)
    {
        field.DestroyBrick(coords, onComplete);
    }
}