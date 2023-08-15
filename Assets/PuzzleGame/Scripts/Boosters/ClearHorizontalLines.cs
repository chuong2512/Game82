using System;
using UnityEngine;

public static class ClearHorizontalLines
{
    public static void Execute(NumberedBrick[,] field, int coordY, int linesCount, Action onComplete)
    {
        int fieldSizeX = field.GetLength(0);

        for (int x = 0; x < fieldSizeX; x++)
        {
            for (int i = 0; i < linesCount; i++)
                field.DestroyBrick(new Vector2Int(x, coordY + i), x < fieldSizeX - 1 && i < linesCount - i ? null : onComplete);
        }
    }
}