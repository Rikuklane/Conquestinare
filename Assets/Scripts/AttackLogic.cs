using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class AttackLogic : MonoBehaviour
{
    public static AttackLogic Instance;
    public Territory selectedTerritory;
    public Territory attackTerritory;

    public GameObject territoryManager;

    public bool isPlacementTurn = true;
    public bool isReorganizeTurn = false;
    public bool isReorganizeTriggered = false;
    public bool canHover = false;

    void Awake()
    {
        Instance = this;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void checkWin()
    {
        Player currentPlayer = Events.RequestPlayer();
        int enemyTerritories = 0;
        int playerTerritories = 0;
        foreach(Transform t in territoryManager.transform)
        {
            if (t.GetComponent<Territory>().player.Name == "neutral") continue;
            if (t.GetComponent<Territory>().player == currentPlayer)
            {
                playerTerritories++;
            } else
            {
                enemyTerritories++;
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
        bool isPlayerTerritory = newSelected.player == Events.RequestPlayer();
        // TODO: disable any click events as well
        if (isReorganizeTriggered)
        {
            return;
        }
        newSelected.UpdateEnemyTerritories();

        if (selectedTerritory == null)
        {
            if (!isPlayerTerritory) return;
            selectedTerritory = newSelected;
            selectedTerritory.ShowAttackOptions();
            AudioController.Instance.warCry.Play();
            return;

        } if (selectedTerritory == newSelected){
            // disable attackOptions
            selectedTerritory.HideAttackOptions();
            selectedTerritory = null;
            return;
        }
        // check if territories are connected
        bool territoriesConnected = selectedTerritory.territories.Contains(newSelected);
        if (!isPlayerTerritory)
        {
            if (selectedTerritory.GetUnitsCount() == 0) return;
            if (!territoriesConnected) return;
            attackTerritory = newSelected;
            AudioController.Instance.warCry.Play();
            AttackGUI.instance.attackButton.gameObject.SetActive(true);
            // attack line only
            selectedTerritory.HideAttackOptions();
            selectedTerritory.waypoint.SetLine(attackTerritory.waypoint.transform.position, true);
        }
        else
        {

            if (isReorganizeTurn && territoriesConnected)
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
        List<UnitCardPresenter> presentUnitsCopy = selectedTerritory.TerritoryGraphics.presentUnits.ToList();
        List<Territory.Unit> unitsCopy = selectedTerritory.units.ToList();

        for (int i = 0; i< presentUnitsCopy.Count; i++)
        {
            UnitCardPresenter card = presentUnitsCopy[i];
            if (card.cardLogic.isSelected)
            {
                card.cardLogic.isSelected = false;
                // add cards to 2nd territory
                attackTerritory.AddCard(card.unitData, unitsCopy[i]);
                // remove cards from 1st territory
                selectedTerritory.RemoveCard(selectedTerritory.TerritoryGraphics.presentUnits.IndexOf(card));
            }
        }
        // disable panel
        AttackGUI.instance.attackButton.gameObject.SetActive(false);
        // can hover
        canHover = true;
        isReorganizeTriggered = false;

        // if triggered from battle turn, change back button behavior
        if(!isReorganizeTurn)
        {
            AttackGUI.instance.ChangeButtonClickAttack(true);
        }
        checkWin();
        // cleanup card colors
        foreach (UnitCardPresenter card in selectedTerritory.TerritoryGraphics.presentUnits)
        {
            card.cardLogic.ChangeInteractable(true);
        }
        // cleanup
        AttackCleanup();


    }

    void ResetLines()
    {
        selectedTerritory.HideAttackOptions();
        attackTerritory.HideAttackOptions();
        attackTerritory.UpdateEnemyTerritories();
        foreach (Territory territory in attackTerritory.territories)
        {
            territory.UpdateEnemyTerritories();
        }
    }

    public void AttackPressed()
    {
        bool isWin = SimulateBattle();

        // winCondition
        if (isWin && selectedTerritory.GetUnitsCount() > 1)
        {
            AudioController.Instance.cardHit.Play();
            AttackGUI.instance.ChangeButtonClickAttack(false);
            TriggerReorganize();
            attackTerritory.player = selectedTerritory.player;
            attackTerritory.TerritoryGraphics.SetColor(selectedTerritory.player.color);
            ResetLines();
            // if player had bonus, remove visuals
            if (attackTerritory.TerritoryGraphics.isShowBonus)
            {
                TerritoryManager.instance.ShowBonus((int)attackTerritory.bonusGroup, false);
            }
            // check if current player got new bonuses
            Events.RequestBonus(Events.RequestPlayer());

            return;
            // transfer 2nd troop over
            //attackTerritory.AddCard(selectedTerritory.presentUnits[1]);
            // selectedTerritory.RemoveCard(1);

        }
        else if (isWin)
        {
            AudioController.Instance.cardHit.Play();
            // Win without territory gain
            attackTerritory.UpdateEnemyTerritories();
            // if player had bonus, remove visuals
            if (attackTerritory.TerritoryGraphics.isShowBonus)
            {
                TerritoryManager.instance.ShowBonus((int)attackTerritory.bonusGroup, false);
            }
            // check if current player got new bonuses
            Events.RequestBonus(Events.RequestPlayer());
        }
        else
        {
            AudioController.Instance.cannon.Play();
            // if player had bonus, remove visuals
            if (selectedTerritory.TerritoryGraphics.isShowBonus)
            {
                TerritoryManager.instance.ShowBonus((int)selectedTerritory.bonusGroup, false);
            }
            // LOSE
            selectedTerritory.UpdateEnemyTerritories();
            selectedTerritory.enemyTerritories.Clear();
        }
        AttackCleanup();

        AttackGUI.instance.AttackCleanup();

        checkWin();
    }

    private void AttackCleanup()
    {
        // cleanup
        selectedTerritory.UpdateTerritoryImage();
        attackTerritory.UpdateTerritoryImage();

        selectedTerritory.HideAttackOptions();
        attackTerritory.HideAttackOptions();

        selectedTerritory.TerritoryGraphics.hideCards();
        attackTerritory.TerritoryGraphics.hideCards();

        selectedTerritory = null;
        attackTerritory = null;
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


