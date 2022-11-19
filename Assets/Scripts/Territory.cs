using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Territory : MonoBehaviour
{
    public Waypoint waypoint;

    public List<Territory> territories = new();
    public List<Vector3> enemyTerritories = new();
    public bool isEnemy;
    public bool isNeutral = false;
    
    public List<UnitCardPresenter> startUnits = new();

    public List<Unit> units = new();
    public class Unit
    {
        public int attack;
        public int health;
    }

    public TerritoryGraphics TerritoryGraphics;
        
    void Awake()
    {

        foreach(Territory area in territories)
        {
            waypoint.routes.Add(area.waypoint);
        }
        waypoint.CreateLines();

        UpdateEnemyTerritories();
    }

    private void Start()
    {
        foreach (UnitCardPresenter unit in startUnits)

        {
            AddCard(unit);
        }
    }

    public void AttackUnit(int index, int damage)
    {
        print(index + " " + damage + " " + units[index].health);
        units[index].health -= damage;
        TerritoryGraphics.presentUnits[index].SetHealth(units[index].health);

        if (units[index].health <= 0)
        {
            RemoveCard(index);
            print("unit died");
        }
    }

    public Unit GetUnitAtIndex(int index)
    {
        return units[index];
    }

    public int GetUnitsCount()
    {
        return units.Count;
    }

    public void AddCard(UnitCardPresenter unit)
    {

        UnitCardPresenter card = Instantiate(unit, GUI.instance.TerritoryHoverPanel.transform);
        card.gameObject.SetActive(false);
        // overwrite scale
        card.transform.localScale = new Vector3(2, 2, 2);

        card.SwitchState(card.CardInTerritory);

        Color color = card.GetComponent<Image>().color;
        color.a = 0.9f;
        card.GetComponent<Image>().color = color;

        //presentUnits.Add(card);
        TerritoryGraphics.presentUnits.Add(card);

        Unit newUnit = new Unit();
        newUnit.attack = unit.unitData.attack;
        newUnit.health = unit.unitData.health;
        units.Add(newUnit);
        UpdateTerritoryImage();

        UpdateEnemyTerritories();

        card.isSelected = false;
        //TODO
    }

    public void RemoveCard(int index)
    {
        units.RemoveAt(index);
        //Destroy(presentUnits[index].gameObject);
        //presentUnits.RemoveAt(index);
        Destroy(TerritoryGraphics.presentUnits[index].gameObject);
        TerritoryGraphics.presentUnits.RemoveAt(index);
        UpdateTerritoryImage();
    }

    public void UpdateTerritoryImage()
    {
        // Summary
        int attack = 0;
        int health = 0;
        foreach(Unit unit in units)
        {
            attack += unit.attack;
            health += unit.health;
        }
        TerritoryGraphics.SetSummaryText(attack + "AD / " + health + "HP");
        // Color
        if (attack == 0 && health == 0)
        {
            isNeutral = true;
            TerritoryGraphics.SetColor(Color.gray);
        }
        else
        {
            isNeutral = false;
            TerritoryGraphics.SetPlayerColor(isEnemy);
        }
    }

    public void ShowAttackOptions()
    {
        waypoint.SetLines(enemyTerritories, true);
    }

    public void HideAttackOptions()
    {
        waypoint.SetLines(enemyTerritories, false);
    }

    public void UpdateEnemyTerritories()
    {
        enemyTerritories.Clear();
        foreach(Territory area in territories)
        {
            if (isEnemy != area.isEnemy || area.isNeutral)
            {
                enemyTerritories.Add(area.waypoint.transform.position);
            }
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(AttackLogic.instance.isPlacementTurn)
            {
                UnitCardPresenter cardSelected = CardHand.Instance.cardSelected;
                if (cardSelected)
                {

                    AddCard(cardSelected);
                    CardHand.Instance.DestroySelected();

                }
            } else
            {
                AttackLogic.instance.SelectTerritory(this);
            }

            //waypoint.ToggleLines();
            TerritoryGraphics.ChangeColor(new Color(200 / 255f, 200 / 255f, 200 / 255f));
        }

    }

}
