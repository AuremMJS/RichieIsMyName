using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scriptable object to store level configuration
[CreateAssetMenu(fileName = "LevelsSO", menuName = "ScriptableObjects/LevelsSO", order = 1)]
public class LevelsSO : ScriptableObject
{
    public LevelConfigurationSO[] levels;
}
