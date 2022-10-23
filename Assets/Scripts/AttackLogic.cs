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
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearLines()
    {
        foreach(Transform child in transform)
        {
            
        }
    }
    public void SelectTerritory(Territory newSelected)
    {
        if(selectedTerritory == null)
        {
            selectedTerritory = newSelected;
            selectedTerritory.showAttackOptions();
            return;

        } else
        {
            if (IsEnemyTerritory(newSelected))
            {
                attackTerritory = newSelected;
                attackButton.gameObject.SetActive(true);
                // attack line only
                selectedTerritory.hideAttackOptions();
                selectedTerritory.waypoint.SetLine(attackTerritory.waypoint.transform.position, true);
            }
            else 
            {
                // change selected
                selectedTerritory.hideAttackOptions();
                newSelected.showAttackOptions();
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
        selectedTerritory.hideAttackOptions();
        attackTerritory.hideAttackOptions();
        attackTerritory.isEnemy = selectedTerritory.isEnemy;
        attackTerritory.UpdateEnemyTerritories();
        foreach (Territory territory in attackTerritory.territories)
        {
            territory.UpdateEnemyTerritories();
        }
    }

    public void AttackPressed()
    {
        // winCondition
        ResetLines();
        attackButton.gameObject.SetActive(false);
        attackTerritory.setColor(selectedTerritory.color);
        selectedTerritory = null;
        attackTerritory = null;
    }

    public void showCards(List<Unit> units, GameObject cardPrefab)
    {
        foreach(Transform child in TerritoryHoverPanel.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(Unit unit in units)
        {
            cardPrefab.GetComponent<UnitCardPresenter>().unitData = unit.unitData;
            GameObject.Instantiate(cardPrefab, TerritoryHoverPanel.transform);

        }
        TerritoryHoverPanel.SetActive(true);
        
    }
    public void hideCards()
    {
        TerritoryHoverPanel.SetActive(false);
    }
}
