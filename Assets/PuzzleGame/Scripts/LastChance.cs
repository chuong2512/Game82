using System;
using UnityEngine.Serialization;

[Serializable]
public class LastChance
{
    public LastChanceType LastChanceType = LastChanceType.Numbers;

    public int MaxNumber = 4;
    public int LinesCount = 1;
}