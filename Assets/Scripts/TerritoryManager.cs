using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerritoryManager : MonoBehaviour
{
    public static TerritoryManager instance;
    public List<Territory> territories = new();
    public List<UnitData> unitsStartPool = new();

    private List<List<UnitData>> playerUnitPools = new();
    private List<int> bonusTerritoryTotals = new();
    private int playerIndex = -1;

    public ScalingAnimation OpenAnimation;
    public ScalingAnimation CloseAnimation;

    public Image iconPrefab;
    public GameObject provinceCanvasPrefab;

    public enum BonusGroup
    {
        LEFT, RIGHT
    }

    private void Awake()
    {
        instance = this;
        for (int i = 0; i< territories.Count - unitsStartPool.Count; i++)
        {
            unitsStartPool.Add(unitsStartPool[0]);
        }
        Events.OnRequestTerritory += GetPlayerTerritoriesCount;
        Events.OnRequestBonus += GetPlayerBonus;

        bonusTerritoryTotals = new List<int>() { 0, 0, 0, 0, 0, 0 };
        foreach (Transform child in transform)
        {
            Territory territory = child.GetComponent<Territory>();
            if (!territory) continue;
            //print(territory.bonusGroup);
            bonusTerritoryTotals[territory.bonusGroup] += 1;
        }
    }
    [ContextMenu("New territories")]
    private void AddTerritories()
    {
        territories.Clear();

        foreach (Transform child in transform)
        {
            print(child.GetComponent<MeshFilter>().mesh.bounds);
            if (child.name.Equals("province "))
            {
                continue;
            }

            Territory territory = child.GetComponent<Territory>();
            if (!territory)
            {
                territory = child.gameObject.AddComponent<Territory>();
                TerritoryGraphics territoryGraphics = child.gameObject.AddComponent<TerritoryGraphics>();
                GameObject canvas = Instantiate(provinceCanvasPrefab, territory.transform.position, Quaternion.identity, territory.transform);
                territory.TerritoryGraphics = territoryGraphics;
                territoryGraphics.iconsParent = canvas.transform.Find("TerritoryIcons").gameObject;
                territoryGraphics.attackImage = canvas.transform.Find("AttackHover").GetComponent<Image>();
                territoryGraphics.defenseImage = canvas.transform.Find("DefenseHover").GetComponent<Image>();
                territoryGraphics.markerImage = canvas.transform.Find("MarkerHover").GetComponent<Image>();
                territoryGraphics.attackImage.enabled = false;
                territoryGraphics.defenseImage.enabled = false;
                territoryGraphics.markerImage.enabled = false;
                territoryGraphics.OpenAnimation = OpenAnimation;
                territoryGraphics.CloseAnimation = CloseAnimation;
            }

            territories.Add(territory);
            bonusTerritoryTotals[(int)territory.bonusGroup] += 1;
        }
        // neighbors
        foreach (Transform child in transform)
        {
            Territory territory = child.GetComponent<Territory>();
            foreach(ProvinceData data in child.GetComponent<ProvinceData>().neighbors)
            {

                territory.territories.Add(data.gameObject.GetComponent<Territory>());
            }
        }
    }

    private int GetPlayerTerritoriesCount(Player player)
    {
        return GetPlayerTerritories(player).Count;
    }
    
    public List<Territory> GetPlayerTerritories(Player player)
    {
        List<Territory> playerTerritories = new List<Territory>();
        foreach(Territory territory in territories)
        {
            if(territory.player == player)
            {
                playerTerritories.Add(territory);
            }
        }
        return playerTerritories;
    }

    private int GetPlayerBonus(Player player)
    {
        List<int> playerBonusTerritories = new List<int>();
        bonusTerritoryTotals.ForEach(b => playerBonusTerritories.Add(0));
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
                bonusTotal += 2;
            }
        }
        return bonusTotal;
    }

    public void ShowBonus(int BonusTypeNumber, bool showBonus)
    {
        int bonusGroup = BonusTypeNumber;
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
        Events.OnRequestTerritory -= GetPlayerTerritoriesCount;
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
        foreach (Territory territory in territories)
        {
            territory.UpdateNeighborTerritories();
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
