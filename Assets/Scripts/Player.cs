using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player
{
    public bool isAlive = true;
    private int _bonusFromAreasController;

    public Player(string playerName, Color color, bool isNpc)
    {
        name = playerName;
        this.color = color;
        this.isNpc = isNpc;
    }

    public string name { get; }

    public Color color { get; }

    public int gold { get; set; }
    public int territories { get; set; }
    public CardData cards { get; set; }
    public bool isNpc { get; }

    // This method defines the value of unit cards given at the start of the round
    public int GetPrestige()
    {
        return (int)(Events.RequestTerritory(this) * 0.5f + Events.RequestBonus(this));
    }
}
