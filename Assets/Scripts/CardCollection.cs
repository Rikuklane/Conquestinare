using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class CardCollection : MonoBehaviour
{
    public static CardCollection Instance;
    public List<UnitData> units;
    public List<SpellData> spells;
    private List<CardData> _cards;
    private readonly Random _random = new();

    private void Awake()
    {
        Instance = this;
        _cards = new List<CardData>(units);
        _cards.AddRange(spells);
    }

    public List<UnitData> GetSelectionOfUnits(int amount)
    {
        return GetSelectionOfCards(units, amount);
    }
    
    public List<CardData> GetSelectionOfCards(int amount)
    {
        return GetSelectionOfCards(_cards, amount);
    }
    
    private List<T> GetSelectionOfCards<T>(IList<T> list, int amount)
    {
        var returnedList = new List<T>();
        Shuffle(list);
        for (int i = 0; i < amount; i++)
        {
            returnedList.Add(list[i]);
        }
        return returnedList;
    }

    private void Shuffle<T>(IList<T> list)
    {
        var n = list.Count;  
        while (n > 1) {  
            n--;  
            var k = _random.Next(n + 1);  
            (list[k], list[n]) = (list[n], list[k]);
        } 
    }
}
