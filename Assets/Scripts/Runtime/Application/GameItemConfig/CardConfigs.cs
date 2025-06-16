using Core;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardConfigs", menuName = "Config/CardConfigs")]
public class CardConfigs : BaseSettings
{
    public List<CardConfig> Cards;
}