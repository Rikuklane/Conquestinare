using System;
using System.Collections;
using System.Collections.Generic;
using CardStates;
using UnityEngine;

public class CardStateController : MonoBehaviour
{
    public static CardStateController Instance;
    public readonly CardInHand CardInHand = new();
    public readonly CardInMarket CardInMarket = new();
    public readonly CardInSelection CardInSelection = new();
    public readonly CardInTerritory CardInTerritory = new();

    private void Awake()
    {
        Instance = this;
    }
}
