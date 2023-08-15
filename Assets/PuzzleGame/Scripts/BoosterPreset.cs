using UnityEngine;

[CreateAssetMenu(fileName = "BoosterPreset", menuName = "Booster Preset")]
public class BoosterPreset : ScriptableObject
{
    public BoosterType Type;
    public Sprite Icon;
    public string Description;
    public Price Price;
    public int CountToBuy = 1;
}
