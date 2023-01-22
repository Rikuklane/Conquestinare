using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
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
        await Task.Delay(1500);
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
        
        float handPower = 0;
        if (cardHand.Count > 0)
        {
            foreach (var card in cardHand)
            {
                if (card.cardData.GetType() == typeof(UnitData))
                {
                    var unit = (UnitData)card.cardData;
                    handPower += CalculateUnitPower(unit.attack, unit.health);
                }
            }
        }

        List<Territory> bestPossibilities = new();
        Territory bestTerritory = npcTerritories.First();

        foreach (var territory in npcTerritories)
        {
            List<Territory> possibilities = FilterBestTerritoriesToAttackFrom(territory, handPower);
            if (possibilities.Count > bestPossibilities.Count)
            {
                bestPossibilities = possibilities;
                bestTerritory = territory;
            }
        }
        
        // place cards from hand
        while (cardHand.Count != 0)
        {
            cardHand[0].SelectCard();
            bestTerritory.MoveCardToTerritory(cardHand[0], bestTerritory.transform.position);
            await Task.Delay(500);
        }

        // attack the possibilities from the territory
        foreach (var territory in bestPossibilities)
        {
            Territory.Unit enemy = territory.GetAttackHealth();
            Territory.Unit bot = bestTerritory.GetAttackHealth();
            if (1.5 * bot.attack <= enemy.health || bot.health <= enemy.attack)
            {
                continue;
            }
            AttackLogic.Instance.SelectTerritory(bestTerritory);
            await Task.Delay(500);
            AttackLogic.Instance.SelectTerritory(territory);
            await Task.Delay(500);
            AttackGUI.instance.attackButton.onClick.Invoke();
            await Task.Delay(3000);
            var units = bestTerritory.TerritoryGraphics.presentUnits.ToList();
            units.OrderBy(unit => unit.attack + unit.health).First().cardLogic.SelectCard();
            await Task.Delay(500);
            AttackGUI.instance.attackButton.onClick.Invoke();
            await Task.Delay(500);
        }

        // TODO while conquering always leave more units in a place where there are more enemy territories around
        
        TurnManager.Instance.TriggerTurnEndStateButton();
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
        // TODO take into account the bonus areas somehow in this method to count as a double value?
        List<Territory> possibleTargets = new();
        foreach (var territoryUnit in territory.units)
        {
            power += CalculateUnitPower(territoryUnit.attack, territoryUnit.health);
        }

        if (territory.enemyTerritories.Count == 0)
        {
            return possibleTargets;
        }

        var orderedEnemyTerritories = territory.enemyTerritories
            .OrderBy(x => CalculateUnitsPowers(x.units));
        foreach (Territory enemyTerritory in orderedEnemyTerritories)
        {
            power = power - CalculateUnitsPowers(enemyTerritory.units) - 2;
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
    
    private float CalculateUnitsPowers(List<Territory.Unit> units)
    {
        float power = 0;
        foreach (var unit in units)
        {
            power += CalculateUnitPower(unit.attack, unit.health);
        }
        return power;
    }

    private float CalculateUnitPower(int attack, int health)
    {
        return (float)(attack + health) / 2;
    }
}
