using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUI : MonoBehaviour
{
    public static GUI instance;

    public Button attackButton;
    public GameObject TerritoryHoverPanel;
    public GameObject ArenaPanel;
    public GameObject ArenaTopPanel;
    public GameObject ArenaBottomPanel;

    public GameObject WinScreen;
    public TextMeshProUGUI WinScreenText;

    void Awake()
    {
        instance = this;
    }

    public void GameOver(bool isWin)
    {
        if (isWin)
        {
            WinScreen.SetActive(true);
            WinScreenText.text = "You Win!";
        } else
        {
            WinScreen.SetActive(true);
            WinScreenText.text = "You lose";
        }
    }

    public void ShowBattle()
    {
        // show panel with cards
        //showCards(selectedTerritory.presentUnits, ArenaBottomPanel);
        //showCards(attackTerritory.presentUnits, ArenaTopPanel);
        //ArenaPanel.gameObject.SetActive(true);
    }

    public void ChangeButtonClickAttack(bool isAttack)
    {
        if (isAttack)
        {
            attackButton.GetComponentInChildren<TextMeshProUGUI>().text = "ATTACK";
            attackButton.onClick.RemoveAllListeners();
            attackButton.onClick.AddListener(AttackLogic.instance.AttackPressed);
        }
        else
        {
            attackButton.GetComponentInChildren<TextMeshProUGUI>().text = "CONFIRM";
            attackButton.onClick.RemoveAllListeners();
            attackButton.onClick.AddListener(AttackLogic.instance.CheckSelected);
        }

    }

    public void AttackCleanup()
    {
        ArenaPanel.gameObject.SetActive(false);
        attackButton.gameObject.SetActive(false);
        // weird fix
        TerritoryHoverPanel.gameObject.SetActive(false);
    }
}
