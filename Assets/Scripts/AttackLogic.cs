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

    public bool isReorganizeTurn = false;
    public bool isReorganizeTriggered = false;
    public bool canHover = false;
    // for simulateBattle logic
    private bool isDefenderTurn = true;
    private int noMaxBattleTurns = 50;

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
        foreach(Transform t in TerritoryManager.instance.transform)
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
        // show panel with cards
        AttackGUI.instance.ShowBattle(selectedTerritory, attackTerritory);
        SimulateBattle();
    }

    public void HandleBattleResult(bool isWin)
    {
        // call handleBattleResult after coroutine finished
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
    
    public bool SimulateBattle()
    {
        if (attackTerritory.GetUnitsCount() <= 0)
        {
            AttackGUI.instance.HideBattle();
            HandleBattleResult(true);
            return true;
        }
        if (selectedTerritory.GetUnitsCount() <= 0)
        {
            AttackGUI.instance.HideBattle();
            HandleBattleResult(false);
            return false;
        }

        if (noMaxBattleTurns > 0)
        {
            StartCoroutine(AttackGUI.instance.AttackAnimation(isDefenderTurn, selectedTerritory, attackTerritory));
        }
        isDefenderTurn = !isDefenderTurn;
        noMaxBattleTurns--;
        return false;
    }
}


