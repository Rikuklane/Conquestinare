using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using CardStates;
using Turns;
using UnityEngine;
using UnityEngine.UI;

public class NpcBehaviour : MonoBehaviour
{
    public static NpcBehaviour Instance;
    public FadeCanvasGroup turnStartScreen;
    private void Awake()
    {
        Instance = this;
    }

    public void PlayerStartTurnActions()
    {
        turnStartScreen.FadeOut();
    }
    
    public void ReceiveUnitsTurnActions()
    {
        TurnManager.Instance.TriggerTurnEndState();
    }
    
    public void MarketTurnActions()
    {
        TurnManager.Instance.TriggerTurnEndStateButton();
    }
    
    public void BattleTurnActions()
    {
        TurnManager.Instance.TriggerTurnEndStateButton();
    }
    
    public void ReOrganizeTurnActions()
    {
        TurnManager.Instance.TriggerTurnEndStateButton();
    }
}
