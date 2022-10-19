using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Game : MonoBehaviour
{
    public static Game instance;
    public Territory selectedTerritory;
    public Territory attackTerritory;

    public Button attackButton;
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
}
