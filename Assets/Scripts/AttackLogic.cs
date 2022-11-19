using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class AttackLogic : MonoBehaviour
{
    public static AttackLogic instance;
    public Territory selectedTerritory;
    public Territory attackTerritory;

    public GameObject territoryManager;

    public bool isPlacementTurn = true;
    public bool isReorganizeTurn = false;
    public bool isReorganizeTriggered = false;
    public bool canHover = false;

    void Awake()
    {
        instance = this;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void checkWin()
    {
        int enemyTerritories = 0;
        int playerTerritories = 0;
        foreach(Transform t in territoryManager.transform)
        {
            if (t.GetComponent<Territory>().isNeutral) continue;
            if (t.GetComponent<Territory>().isEnemy)
            {
                enemyTerritories++;
            } else
            {
                playerTerritories++;
            }
        }
        if (enemyTerritories == 0)
        {
            AttackGUI.instance.GameOver(true);
        }
        if (playerTerritories == 0)
        {
            AttackGUI.instance.GameOver(false);
        }
    }

    public void SelectTerritory(Territory newSelected)
    {
        // TODO: disable any click events as well
        if (isReorganizeTriggered)
        {
            return;
        }
        newSelected.UpdateEnemyTerritories();

        if (selectedTerritory == null)
        {
            selectedTerritory = newSelected;
            selectedTerritory.ShowAttackOptions();
            return;

        } else if (selectedTerritory == newSelected){
            // disable attackOptions
            selectedTerritory.HideAttackOptions();
            selectedTerritory = null;
            return;
        }
        else
        {
            if (IsEnemyTerritory(newSelected))
            {
                if (selectedTerritory.GetUnitsCount() == 0) return;
                attackTerritory = newSelected;
                AttackGUI.instance.attackButton.gameObject.SetActive(true);
                // attack line only
                selectedTerritory.HideAttackOptions();
                selectedTerritory.waypoint.SetLine(attackTerritory.waypoint.transform.position, true);
            }
            else
            {

                if (isReorganizeTurn)
                {
                    attackTerritory = newSelected;
                    TriggerReorganize();

                } else
                {
                    // change selected
                    selectedTerritory.HideAttackOptions();
                    newSelected.ShowAttackOptions();
                    selectedTerritory = newSelected;
                }
            }

        }

    }
    private void TriggerReorganize()
    {
        isReorganizeTriggered = true;
        attackTerritory.TerritoryGraphics.hideCards();

        canHover = false;

        // reorganizeTurn
        // confirm button
        AttackGUI.instance.attackButton.gameObject.SetActive(true);

        // add cards to panel
        selectedTerritory.TerritoryGraphics.showCards();




    }
    

    public void CheckSelected()
    {
        // check which cards selected 
        // TODO: check that 1 card not selected
        // TODO: update icons
        foreach (UnitCardPresenter card in selectedTerritory.TerritoryGraphics.presentUnits.ToList())
        {
            if (card.isSelected)
            {
                card.isSelected = false;
                attackTerritory.AddCard(card.unitData);
                // remove cards from 1st territory

                // add cards to 2nd territory
                selectedTerritory.RemoveCard(selectedTerritory.TerritoryGraphics.presentUnits.IndexOf(card));
            }
        }
        // disable panel
        selectedTerritory.TerritoryGraphics.hideCards();
        AttackGUI.instance.attackButton.gameObject.SetActive(false);
        // can hover
        canHover = true;
        isReorganizeTriggered = false;

        // if triggered from battle turn, change back button behavior
        if(!isReorganizeTurn)
        {
            AttackGUI.instance.ChangeButtonClickAttack(true);
        }

    }

    bool IsEnemyTerritory(Territory territory)
    {
        return selectedTerritory.enemyTerritories.Contains(territory.transform.position)
            && selectedTerritory.TerritoryGraphics.color != territory.TerritoryGraphics.color;
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

        selectedTerritory.UpdateTerritoryImage();
        attackTerritory.UpdateTerritoryImage();

        // winCondition
        if (isWin && selectedTerritory.GetUnitsCount() > 1)
        {
            AttackGUI.instance.ChangeButtonClickAttack(false);
            TriggerReorganize();
            attackTerritory.TerritoryGraphics.SetColor(selectedTerritory.TerritoryGraphics.color);
            selectedTerritory.UpdateTerritoryImage();
            ResetLines();

            return;
            // transfer 2nd troop over
            //attackTerritory.AddCard(selectedTerritory.presentUnits[1]);
            // selectedTerritory.RemoveCard(1);

        }
        else if (isWin)
        {
            // Win without territory gain
            selectedTerritory.HideAttackOptions();
            attackTerritory.HideAttackOptions();
            attackTerritory.UpdateTerritoryImage();
        }
        else 
        { 
            // LOSE
            selectedTerritory.HideAttackOptions();
            attackTerritory.HideAttackOptions();
            selectedTerritory.UpdateTerritoryImage();
            selectedTerritory.UpdateEnemyTerritories();
            selectedTerritory.enemyTerritories.Clear();
        }
        // cleanup
        selectedTerritory.TerritoryGraphics.hideCards();
        attackTerritory.TerritoryGraphics.hideCards();
        
        selectedTerritory = null;
        attackTerritory = null;

        AttackGUI.instance.AttackCleanup();

        checkWin();
    }

    private bool SimulateBattle()
    {
        // show panel with cards
        AttackGUI.instance.ShowBattle();
        int playerCards = selectedTerritory.units.Count;
        int enemyCards = attackTerritory.units.Count;

        // neutral territory
        if(enemyCards==0)
        {
            return true;
        }

        print(playerCards + " " + enemyCards);

        int i = 0; // dont want to write if true
        // less than 50 turns
        while (i<50)
        {
            // defence attacks
            int playerAttack = attackTerritory.GetUnitAtIndex(0).attack;
            selectedTerritory.AttackUnit(0, playerAttack);

            if (selectedTerritory.GetUnitsCount() <= 0)
            {
                return false;
            }

            // attacker attacks
            int enemyAttack = selectedTerritory.GetUnitAtIndex(0).attack;
            attackTerritory.AttackUnit(0, enemyAttack);

            if (attackTerritory.GetUnitsCount() <= 0)
            {
                return true;
            }
            print("end of turn " + i.ToString());
            i++;
        }
        return false;
    }
}


