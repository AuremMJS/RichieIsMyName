using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// Scriptable object to store level configuration
[CreateAssetMenu(fileName = "LevelConfigurationSO", menuName = "ScriptableObjects/LevelConfigurationSO", order = 1)]
public class LevelConfigurationSO : ScriptableObject
{
    public int Rows;
    public int Columns;

    public void OnValidate()
    {
    }
}
