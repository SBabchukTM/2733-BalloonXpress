using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Config", menuName = "Config/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public int Bet = 0;
    public List<int> Prizes;
    public float MoveDuration = 5f;
    public float LevelTime = 5f;
    public Sprite CardSprite;
}