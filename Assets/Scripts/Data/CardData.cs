using System.Collections;
using System.Collections.Generic;
using CardStates;
using UnityEngine;

public class CardData : ScriptableObject
{
    public string title;
    [Multiline]
    public string description;
    public int cost;
    public AbstractCardState State = null;
}
