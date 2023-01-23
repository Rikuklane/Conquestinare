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
    public Player attackedPlayer =  null;

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
        foreach(Territory t in TerritoryManager.instance.territories)
        {
            if (t.player.name == "neutral") continue;
            if (t.player == currentPlayer)
            {
                playerTerritories++;
            } else if (attackedPlayer != null && t.player == attackedPlayer)
            {
                enemyTerritories++;
            }
        }
        if (enemyTerritories == 0)
        {
            AttackGUI.instance.GameOver(attackedPlayer);
        }
        if (playerTerritories == 0)
        {
            AttackGUI.instance.GameOver(currentPlayer);
        }
    }

    public void DeselectAll()
    {
        if (selectedTerritory) selectedTerritory.HideAttackOptions(isReorganizeTurn);
        if (attackTerritory) attackTerritory.HideAttackOptions(isReorganizeTurn);
        selectedTerritory = null;
        attackTerritory = null;
        AttackGUI.instance.AttackCleanup();
        
    }

    public void SelectTerritory(Territory newSelected)
    {
        bool isPlayerTerritory = newSelected.player == Events.RequestPlayer();
        // TODO: disable any click events as well
        if (isReorganizeTriggered)
        {
            return;
        }
        newSelected.UpdateNeighborTerritories();
        // first select
        if (selectedTerritory == null)
        {
            // just in case
            attackTerritory = null;
            if (!isPlayerTerritory) return;
            selectedTerritory = newSelected;
            selectedTerritory.ShowAttackOptions(isReorganizeTurn);
            AudioController.Instance.warCry.Play();
        // selected starting territory - deselect all
        } else if (newSelected == selectedTerritory){
            // disable attackOptions
            DeselectAll();
        }
        else {
            if (isPlayerTerritory)
            {
                if (isReorganizeTurn)
                {
                    attackTerritory = newSelected;
                    attackedPlayer = attackTerritory.player;
                    TriggerReorganize();
                }
                else
                {
                    // change current selected
                    DeselectAll();
                    selectedTerritory = newSelected;
                    newSelected.ShowAttackOptions(isReorganizeTurn);
                }
            }
            else
            {
                // check if territories are connected
                bool isTerritoryNeighbor = selectedTerritory.territories.Contains(newSelected);
                // cant attack if not neighbor || bug || cant manipulate other territories in reorganize turn ||  bug fix, cant attack enemy territory with enemy territory

                if (!isTerritoryNeighbor || selectedTerritory.GetUnitsCount() == 0 || isReorganizeTurn || selectedTerritory.player == newSelected.player)
                {
                    DeselectAll();
                // TODO cant attack neutral with only 1 troop
                } //else if (newSelected.player.Name == "neutral" && selectedTerritory.GetUnitsCount() == 1) return;
                else
                {
                    attackTerritory = newSelected;
                    attackedPlayer = attackTerritory.player;
                    AudioController.Instance.warCry.Play();
                    AttackGUI.instance.attackButton.gameObject.SetActive(true);
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
        selectedTerritory.HideAttackOptions(isReorganizeTurn);
        attackTerritory.HideAttackOptions(isReorganizeTurn);
        attackTerritory.UpdateNeighborTerritories();
        foreach (Territory territory in attackTerritory.territories)
        {
            territory.UpdateNeighborTerritories();
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
        checkWin();
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
            attackTerritory.UpdateNeighborTerritories();
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
            selectedTerritory.HideAttackOptions(isReorganizeTurn);
            selectedTerritory.UpdateNeighborTerritories();
            selectedTerritory.enemyTerritories.Clear();
        }
        AttackCleanup();

        AttackGUI.instance.AttackCleanup();

        checkWin();
    }

    private void AttackCleanup()
    {
        // cleanup
        selectedTerritory.HideAttackOptions(isReorganizeTurn);
        attackTerritory.HideAttackOptions(isReorganizeTurn);

        selectedTerritory.UpdateTerritoryImage();
        attackTerritory.UpdateTerritoryImage();

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


