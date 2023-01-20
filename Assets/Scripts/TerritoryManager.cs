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
    private List<int> bonusTerritoryTotals = new List<int>() { 0, 0 };
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
        Events.OnRequestTerritory += GetPlayerTerritories;
        Events.OnRequestBonus += GetPlayerBonus;

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
            else
            {
                print("x" + child.name + 'x');
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
                territoryGraphics.attackImage.enabled = false;
                territoryGraphics.defenseImage.enabled = false;
                territoryGraphics.OpenAnimation = OpenAnimation;
                territoryGraphics.CloseAnimation = CloseAnimation;
            }

            territories.Add(territory);
            bonusTerritoryTotals[(int)territory.bonusGroup] += 1;
        }
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
                bonusTotal += 2;
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
