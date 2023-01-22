using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class Player
{
    public bool isAlive = true;
    private int _bonusFromAreasController;
    private Random _random = new Random();
    private Color _color;
    private bool _isNpc;
    
    public Player(string playerName)
    {
        Name = playerName;
        _color.b = (float) _random.NextDouble();
        _color.r = (float) _random.NextDouble();
        _color.g = (float) _random.NextDouble();
        _color.a = 1;
    }
    
    public Player(string playerName, bool isNpc)
    {
        Name = playerName;
        _color.b = (float) _random.NextDouble();
        _color.r = (float) _random.NextDouble();
        _color.g = (float) _random.NextDouble();
        _color.a = 1;
        _isNpc = isNpc;
    }

    public Player(string playerName, Color color)
    {
        Name = playerName;
        _color = color;
    }

    public string Name { get; }

    public Color color => _color;
    public int gold { get; set; }
    public int territories { get; set; }
    public CardData cards { get; set; }
    public bool isNpc => _isNpc;

    // This method defines the value of unit cards given at the start of the round
    public int GetPrestige()
    {
        return (int)(Events.RequestTerritory(this) * 0.5f + Events.RequestBonus(this));
    }
}
