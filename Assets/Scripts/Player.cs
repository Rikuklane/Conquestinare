using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player
{
    private int _bonusFromAreasController;

    public Player(string playerName)
    {
        Name = playerName;
    }

    public string Name { get; }
    public int Gold { get; set; }
    public int Territories { get; set; }
    public CardData Cards { get; set; }

    // This method defines the value of unit cards given at the start of the round
    public int GetPrestige()
    {
        return (int)(Territories * 0.5f + _bonusFromAreasController);
    }
}
