using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AttackLogic : MonoBehaviour
{
    public static AttackLogic instance;
    public Territory selectedTerritory;
    public Territory attackTerritory;

    public Button attackButton;
    public GameObject TerritoryHoverPanel;
    public GameObject ArenaPanel;
    public GameObject ArenaTopPanel;
    public GameObject ArenaBottomPanel;

    public GameObject cardPrefab;


    void Awake()
    {
        instance = this;
    }

    public void SelectTerritory(Territory newSelected)
    {
        if(selectedTerritory == null)
        {
            selectedTerritory = newSelected;
            selectedTerritory.ShowAttackOptions();
            return;

        } else
        {
            if (IsEnemyTerritory(newSelected))
            {
                attackTerritory = newSelected;
                attackButton.gameObject.SetActive(true);
                // attack line only
                selectedTerritory.HideAttackOptions();
                selectedTerritory.waypoint.SetLine(attackTerritory.waypoint.transform.position, true);
            }
            else 
            {
                // change selected
                selectedTerritory.HideAttackOptions();
                newSelected.ShowAttackOptions();
                selectedTerritory = newSelected;
            }
        }

    }

    bool IsEnemyTerritory(Territory territory)
    {
        return selectedTerritory.enemyTerritories.Contains(territory.transform.position)
            && selectedTerritory.color != territory.color;
    }

    void ResetLines()
    {
        selectedTerritory.HideAttackOptions();
        attackTerritory.HideAttackOptions();
        attackTerritory.isEnemy = selectedTerritory.isEnemy;
        attackTerritory.UpdateEnemyTerritories();
        foreach (Territory territory in attackTerritory.territories)
        {
            territory.UpdateEnemyTerritories();
        }
    }

    public void AttackPressed()
    {
        bool isWin = SimulateBattle();

        selectedTerritory.SetSummary();
        attackTerritory.SetSummary();

        // winCondition
        if (isWin && selectedTerritory.GetUnits().Count > 1)
        {
            attackTerritory.SetColor(selectedTerritory.color);
            ResetLines();
            // transfer 2nd troop over
            attackTerritory.AddCard(selectedTerritory.presentUnits[01]);
            // remove old
            selectedTerritory.GetUnits().RemoveAt(1);
            Destroy(selectedTerritory.presentUnits[1].gameObject);
            selectedTerritory.presentUnits.RemoveAt(1);
            selectedTerritory.SetSummary();
        }
        else if (isWin)
        {
            // Win without territory gain
            selectedTerritory.HideAttackOptions();
            attackTerritory.HideAttackOptions();
            attackTerritory.SetColor(Color.gray);
            attackTerritory.enemyTerritories = null;
        }
        else 
        { 
            // LOSE
            selectedTerritory.HideAttackOptions();
            attackTerritory.HideAttackOptions();
            selectedTerritory.SetColor(Color.gray);
            selectedTerritory.enemyTerritories = null;
        }
        // cleanup
        hideCards(selectedTerritory.presentUnits);
        hideCards(attackTerritory.presentUnits);
        ArenaPanel.gameObject.SetActive(false);
        attackButton.gameObject.SetActive(false);
        selectedTerritory = null;
        attackTerritory = null;

        // weird fix
        TerritoryHoverPanel.gameObject.SetActive(false);


    }

    private bool SimulateBattle()
    {
        // show panel with cards
        showCards(selectedTerritory.presentUnits, ArenaBottomPanel);
        showCards(attackTerritory.presentUnits, ArenaTopPanel);
        ArenaPanel.gameObject.SetActive(true);
        int playerCards = selectedTerritory.presentUnits.Count;
        int enemyCards = attackTerritory.presentUnits.Count;

        print(playerCards + " " + enemyCards);
        print(selectedTerritory.GetUnits()[0].attack);
        print(selectedTerritory.GetUnits()[0].health);

        print(attackTerritory.GetUnits()[0].attack);
        print(attackTerritory.GetUnits()[0].health);

        int i = 0; // dont want to write if true
        while (i<10)
        {
            // defence attacks
            int playerAttack = attackTerritory.GetUnits()[0].attack;
            selectedTerritory.GetUnits()[0].health -= playerAttack;
            selectedTerritory.presentUnits[0].SetHealth(selectedTerritory.GetUnits()[0].health);



            if (selectedTerritory.GetUnits()[0].health <= 0)
            {
                selectedTerritory.GetUnits().RemoveAt(0);
                Destroy(selectedTerritory.presentUnits[0].gameObject);
                selectedTerritory.presentUnits.RemoveAt(0);

                playerCards--;
                print("attacker died");
            }

            if (selectedTerritory.presentUnits.Count <= 0)
            {
                return false;
            }

            // attacker attacks
            int enemyAttack = selectedTerritory.GetUnits()[0].attack;
            attackTerritory.GetUnits()[0].health -= enemyAttack;
            attackTerritory.presentUnits[0].SetHealth(attackTerritory.GetUnits()[0].health);

            if (attackTerritory.GetUnits()[0].health <= 0)
            {
                attackTerritory.GetUnits().RemoveAt(0);
                Destroy(attackTerritory.presentUnits[0].gameObject);
                attackTerritory.presentUnits.RemoveAt(0);


                enemyCards--;
                print("defence died");

            }

            if (attackTerritory.presentUnits.Count <= 0)
            {
                return true;
            }
            print("end of turn " + i.ToString());
            i++;
        }

        return false;



    }

    public void showCards(List<UnitCardPresenter> units, GameObject parent)
    {
        //foreach(Transform child in parent.transform)
        ///{
        //    Destroy(child.gameObject);
        //}
        foreach(UnitCardPresenter unit in units)
        {
            //unit.transform.parent = parent.transform;
            unit.gameObject.SetActive(true);
        }
        TerritoryHoverPanel.SetActive(true);
        
    }
    public void hideCards(List<UnitCardPresenter> units)
    {
        foreach (UnitCardPresenter unit in units)
        {
            //unit.transform.parent = parent.transform;
            unit.gameObject.SetActive(false);
        }
        TerritoryHoverPanel.SetActive(false);
    }


}

public class Unit
{
    public int attack;
    public int health;
}
