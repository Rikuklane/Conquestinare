using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private string _playerName;
    private int _territoriesOwned;
    private int _bonusFromAreasController;
    private int _gold;

    public Player(string playerName)
    {
        _playerName = playerName;
    }

    public int Gold
    {
        get => _gold;
        set => _gold = value;
    }
    
    public int Territories
    {
        get => _territoriesOwned;
        set => _territoriesOwned = value;
    }
    
    // This method defines the value of unit cards given at the start of the round
    public int GetPrestige()
    {
        return (int)(Territories * 0.5f + _bonusFromAreasController);
    }
}
