using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class Player
{
    private int _bonusFromAreasController;
    private Random _random = new Random();
    private Color _color;

    public Player(string playerName)
    {
        Name = playerName;
        _color.b = (float) _random.NextDouble();
        _color.r = (float) _random.NextDouble();
        _color.g = (float) _random.NextDouble();
        _color.a = 1;
    }

    public Player(string playerName, Color color)
    {
        Name = playerName;
        _color = color;
    }

    public string Name { get; }

    public Color Color => _color;
    public int Gold { get; set; }
    public int Territories { get; set; }
    public CardData Cards { get; set; }

    // This method defines the value of unit cards given at the start of the round
    public int GetPrestige()
    {
        return (int)(Territories * 0.5f + _bonusFromAreasController);
    }
}
