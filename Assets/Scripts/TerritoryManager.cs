using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryManager : MonoBehaviour
{
    public static TerritoryManager instance;
    public List<Territory> territories = new();
    public List<UnitData> unitsStartPool = new();

    private List<List<UnitData>> playerUnitPools = new();
    private List<int> bonusTerritoryTotals = new List<int>() { 0, 0 };
    private int playerIndex = -1;

    public enum BonusGroup
    {
        LEFT, RIGHT
    }

    private void Awake()
    {
        instance = this;
        territories.Clear();
        foreach (Transform child in transform)
        {
            Territory territory = child.GetComponent<Territory>();
            territories.Add(territory);
            bonusTerritoryTotals[(int)territory.bonusGroup] += 1;
        }
        Events.OnRequestTerritory += GetPlayerTerritories;
        Events.OnRequestBonus += GetPlayerBonus;

    }

    private int GetPlayerTerritories(Player player)
    {
        int terrAmount = 0;
        foreach(Territory territory in territories)
        {
            if(territory.player == player)
            {
                terrAmount++;
            }
        }
        return terrAmount;
    }

    private int GetPlayerBonus(Player player)
    {
        List<int> playerBonusTerritories = new List<int>() { 0, 0};
        foreach (Territory territory in territories)
        {
            if (territory.player == player)
            {
                playerBonusTerritories[(int)territory.bonusGroup] += 1;

            }
        }
        int bonusTotal = 0;
        for(int i = 0; i < bonusTerritoryTotals.Count; i++)
        {
            if(bonusTerritoryTotals[i] == playerBonusTerritories[i])
            {
                // player has bonus
                ShowBonus(i, true);
                bonusTotal += 5;
            }
        }
        return bonusTotal;
    }

    public void ShowBonus(int BonusTypeNumber, bool showBonus)
    {
        BonusGroup bonusGroup = (BonusGroup)BonusTypeNumber;
        foreach (Territory territory in territories)
        {
            if (territory.bonusGroup == bonusGroup)
            {
                territory.TerritoryGraphics.ShowBonus(showBonus);

            }
        }
    }

    private void OnDestroy()
    {
        Events.OnRequestTerritory -= GetPlayerTerritories;
        Events.OnRequestBonus += GetPlayerBonus;
    }

    public void RandomShuffleTerritories(Player[] players)
    {
        foreach(Player player in players)
        {
            playerUnitPools.Add(new List<UnitData>(unitsStartPool));
        }
        Shuffle(territories);
        foreach(Territory territory in territories)
        {
            Player player = GetNextPlayer(players);
            List<UnitData> territoryUnits = GetPlayerUnits();
            territory.player = player;
            territory.startUnits = territoryUnits;
            territory.AddUnits();
        }
    }

    private void Shuffle(List<Territory> territories)
    {
        //https://stackoverflow.com/questions/273313/randomize-a-listt
        int n = territories.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            (territories[k], territories[n]) = (territories[n], territories[k]);
        }
    }

    private Player GetNextPlayer(Player[] players)
    {
        
        if(playerIndex + 1 >= players.Length)
        {
            playerIndex = -1;
        }
        return players[++playerIndex];
    }
    private List<UnitData> GetPlayerUnits()
    {
        List<UnitData> playerUnitPool = playerUnitPools[playerIndex];
        int randomI = Random.Range(0, playerUnitPool.Count);
        UnitData playerUnit = playerUnitPool[randomI];
        playerUnitPool.RemoveAt(randomI);
        return new List<UnitData>(){playerUnit};
    }
}
