using System.Collections;
using System.Collections.Generic;
using CardStates;
using UnityEngine;

public class CardData : ScriptableObject
{
    public string title;
    public string description;
    public int cost;
    public AbstractCardState State = null;
}
