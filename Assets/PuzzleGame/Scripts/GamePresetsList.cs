using UnityEngine;

[CreateAssetMenu(fileName = "GamePresetsList", menuName = "Game Presets List")]
public class GamePresetsList : ScriptableObject
{
    public GamePreset[] presets;
}