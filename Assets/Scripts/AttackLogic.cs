using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

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

    public GameObject territoryManager;

    public bool isPlacementTurn = true;
    public bool isReorganizeTurn = false;
    public bool isReorganizeTriggered = false;
    public bool canHover = false;

    public GameObject WinScreen;
    public TextMeshProUGUI WinScreenText;

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
            WinScreen.SetActive(true);
            WinScreenText.text = "You Win!";
        }
        if (playerTerritories == 0)
        {
            WinScreen.SetActive(true);
            WinScreenText.text = "You lose";
        }
    }

    public void SelectTerritory(Territory newSelected)
    {
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
                if (selectedTerritory.GetUnits().Count == 0) return;
                attackTerritory = newSelected;
                attackButton.gameObject.SetActive(true);
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
        hideCards(attackTerritory.presentUnits);

        canHover = false;

        // reorganizeTurn
        // confirm button
        attackButton.gameObject.SetActive(true);

        // add cards to panel
        showCards(selectedTerritory.presentUnits, TerritoryHoverPanel);




    }
    public void ChangeButtonClickAttack(bool isAttack)
    {
        if (isAttack)
        {
            attackButton.GetComponentInChildren<TextMeshProUGUI>().text = "ATTACK";
            attackButton.onClick.RemoveAllListeners();
            attackButton.onClick.AddListener(AttackPressed);
        } else
        {
            attackButton.GetComponentInChildren<TextMeshProUGUI>().text = "CONFIRM";
            attackButton.onClick.RemoveAllListeners();
            attackButton.onClick.AddListener(CheckSelected);
        }

    }

    public void CheckSelected()
    {
        // check which cards selected 
        // TODO: check that 1 card not selected
        // TODO: update icons
        foreach (UnitCardPresenter card in selectedTerritory.presentUnits.ToList())
        {
            if (card.isSelected)
            {
                card.isSelected = false;
                attackTerritory.AddCard(card);
                // remove cards from 1st territory

                // add cards to 2nd territory
                selectedTerritory.RemoveCard(selectedTerritory.presentUnits.IndexOf(card));
            }
        }
        // disable panel
        hideCards(selectedTerritory.presentUnits);
        attackButton.gameObject.SetActive(false);
        // can hover
        canHover = true;
        isReorganizeTriggered = false;

        // if triggered from battle turn, change back button behavior
        if(!isReorganizeTurn)
        {
            ChangeButtonClickAttack(true);
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

        selectedTerritory.UpdateTerritoryImage();
        attackTerritory.UpdateTerritoryImage();

        // winCondition
        if (isWin && selectedTerritory.GetUnits().Count > 1)
        {
            ChangeButtonClickAttack(false);
            TriggerReorganize();
            attackTerritory.SetColor(selectedTerritory.color);
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
        hideCards(selectedTerritory.presentUnits);
        hideCards(attackTerritory.presentUnits);
        ArenaPanel.gameObject.SetActive(false);
        attackButton.gameObject.SetActive(false);
        selectedTerritory = null;
        attackTerritory = null;

        // weird fix
        TerritoryHoverPanel.gameObject.SetActive(false);

        checkWin();
    }

    private bool SimulateBattle()
    {
        // show panel with cards
        //showCards(selectedTerritory.presentUnits, ArenaBottomPanel);
        //showCards(attackTerritory.presentUnits, ArenaTopPanel);
        //ArenaPanel.gameObject.SetActive(true);
        int playerCards = selectedTerritory.presentUnits.Count;
        int enemyCards = attackTerritory.presentUnits.Count;

        if(enemyCards==0)
        {
            return true;
        }

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
                selectedTerritory.RemoveCard(0);

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
                attackTerritory.RemoveCard(0);

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
