using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryManager : MonoBehaviour
{
    public List<Territory> territories = new();
    public List<UnitData> unitsStartPool = new();

    private List<List<UnitData>> playerUnitPools = new();

    private int playerIndex = -1;

    private void Awake()
    {
        territories.Clear();
        foreach (Transform child in transform)
        {
            territories.Add(child.GetComponent<Territory>());
        }
        
    }
    void Start()
    {
        RandomShuffleTerritories(Turns.TurnManager.Instance._players);
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
            Territory value = territories[k];
            territories[k] = territories[n];
            territories[n] = value;
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
        print(randomI);
        print(playerUnitPool.Count);
        UnitData playerUnit = playerUnitPool[randomI];
        playerUnitPool.RemoveAt(randomI);
        return new List<UnitData>(){playerUnit};
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
