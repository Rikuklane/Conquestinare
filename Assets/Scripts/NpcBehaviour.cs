using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;
using Turns;
using UnityEngine;
using UnityEngine.UI;

public class NpcBehaviour : MonoBehaviour
{
    public static NpcBehaviour Instance;
    public FadeCanvasGroup turnStartScreen;
    private int _currentTime;
    private int _waitLimitBetweenActions;
    private void Awake()
    {
        Instance = this;
    }

    public async void PlayerStartTurnActions()
    {
        await Task.Delay(700);
        turnStartScreen.FadeOut();
    }
    
    public async void ReceiveUnitsTurnActions()
    {
        await Task.Delay(700);
        var item = CardSelector.Instance.selectedUnits.OrderByDescending(x => x.unitData.cost).First();
        item.cardLogic.SelectCard();
    }
    
    public async void MarketTurnActions()
    {
        var units = CardSelector.Instance.selectedUnits;
        foreach (var unit in units)
        {
            await Task.Delay(700);
            unit.cardLogic.SelectCard();
        }

        // TODO do spells later
        var spells = CardSelector.Instance.selectedSpells;
        await Task.Delay(500);
        TurnManager.Instance.TriggerTurnEndStateButton();
    }
    
    public void BattleTurnActions()
    {
        // TurnManager.Instance.TriggerTurnEndStateButton();
    }
    
    public void ReOrganizeTurnActions()
    {
        // TurnManager.Instance.TriggerTurnEndStateButton();
    }
}
