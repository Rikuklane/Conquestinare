using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
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
    
    public async void BattleTurnActions()
    {
        var npc = Events.RequestPlayer();
        var cardHand = CardHand.Instance.cardHands[npc.Name];
        var npcTerritories = TerritoryManager.instance.GetPlayerTerritories(npc);
        var enemyTerritories = TerritoryManager.instance.territories.Except(npcTerritories);
        
        float handPower = 0;
        if (cardHand.Count > 0)
        {
            foreach (var card in cardHand)
            {
                if (card.cardData.GetType() == typeof(UnitData))
                {
                    var unit = (UnitData)card.cardData;
                    handPower += calculateUnitPower(unit.attack, unit.health);
                }
            }
        }

        foreach (var territory in npcTerritories)
        {
            // TODO take into account possibilities
            List<Territory> possibilities = FilterBestTerritoriesToAttackFrom(territory, handPower);
        }
        

        npcTerritories.OrderByDescending(x => FilterBestTerritoriesToAttackFrom(x, handPower));
        // await Task.Delay(300);
        // TODO implement battle
        //  - get rid of cards in hand
        //  - conquer some territories
        //  - while conquering always leave more units in a place where there are more enemy territories around

        // TODO add some logic to look which of the areas are most valuable to conquer
        //  - most likely by bonus groups that the ai has
        // TurnManager.Instance.TriggerTurnEndStateButton();
    }
    
    public async void ReOrganizeTurnActions()
    {
        await Task.Delay(500);
        var npc = Events.RequestPlayer();
        var npcTerritories = TerritoryManager.instance.GetPlayerTerritories(npc).OrderByDescending(x => x.units.Count);
        var first = npcTerritories.First();
        var last = npcTerritories.Last();
        while (first.units.Count > last.units.Count + 1)
        {
            int difference = first.units.Count - last.units.Count;
            if (difference % 2 == 1)
            {
                difference -= 1;
            }
            // TODO transfer units instead of this if below
            if (difference > 1)
            {
                break;
            }
            npcTerritories = npcTerritories.OrderByDescending(x => x.units.Count);
            first = npcTerritories.First();
            last = npcTerritories.Last();
        }
        // TODO add some logic to look which of the areas are most valuable to protect
        TurnManager.Instance.TriggerTurnEndStateButton();
    }

    private List<Territory> FilterBestTerritoriesToAttackFrom(Territory territory, float power)
    {
        List<Territory> possibleTargets = new();
        foreach (var territoryUnit in territory.units)
        {
            power += calculateUnitPower(territoryUnit.attack, territoryUnit.health);
        }

        if (territory.enemyTerritories.Count == 0)
        {
            return possibleTargets;
        }

        var orderedEnemyTerritories = territory.enemyTerritories
            .OrderBy(x => calculateUnitsPowers(x.units));
        foreach (Territory enemyTerritory in orderedEnemyTerritories)
        {
            power = power - calculateUnitsPowers(enemyTerritory.units) - 1.5f;
            if (power > 0)
            {
                possibleTargets.Add(enemyTerritory);
            }
            else
            {
                break;
            }
        }
        return possibleTargets;
    }
    
    private float calculateUnitsPowers(List<Territory.Unit> units)
    {
        float power = 0;
        foreach (var unit in units)
        {
            power += calculateUnitPower(unit.attack, unit.health);
        }
        return power;
    }

    private float calculateUnitPower(int attack, int health)
    {
        return (float)(attack + health) / 2;
    }
}
