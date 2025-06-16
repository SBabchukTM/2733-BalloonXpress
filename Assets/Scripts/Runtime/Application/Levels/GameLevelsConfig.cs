using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameLevelsConfig", menuName = "Config/GameLevelsConfig")]
public class GameLevelsConfig : ScriptableObject
{
    public List<LevelConfig> LevelConfigs;
}
