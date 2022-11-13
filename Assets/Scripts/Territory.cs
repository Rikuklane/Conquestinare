using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Territory : MonoBehaviour
{
    public Waypoint waypoint;
    public Color color;

    public Color playerColor;
    public Color enemyColor;

    public List<Territory> territories = new();
    public List<Vector3> enemyTerritories = new();
    public bool isEnemy;
    public bool isNeutral = false;
    
    public List<UnitCardPresenter> startUnits = new();
    public List<UnitCardPresenter> presentUnits = new();
    public List<Unit> units = new();

    public TextMeshProUGUI summaryText;
    
    private SpriteRenderer spriteRenderer;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(color != null) spriteRenderer.material.color = color;

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

    public List<Unit> GetUnits()
    {
        return units;
    }

    public void AddCard(UnitCardPresenter unit)
    {

        UnitCardPresenter card = Instantiate(unit, AttackLogic.instance.TerritoryHoverPanel.transform);
        card.gameObject.SetActive(false);
        // overwrite scale
        card.transform.localScale = new Vector3(2, 2, 2);

        card.SwitchState(card.CardInTerritory);

        Color color = card.GetComponent<Image>().color;
        color.a = 0.9f;
        card.GetComponent<Image>().color = color;

        presentUnits.Add(card);

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
        Destroy(presentUnits[index].gameObject);
        presentUnits.RemoveAt(index);
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
        summaryText.text = attack + "AD / " + health + "HP";
        // Color
        if (attack == 0 && health == 0)
        {
            isNeutral = true;
            SetColor(Color.gray);
        }
        else if (isEnemy)
        {
            isNeutral = false;
            SetColor(enemyColor);
        }
        else
        {
            isNeutral = false;
            SetColor(playerColor);
        }
    }

    public void SetColor(Color _color)
    {
        color = _color;
        spriteRenderer.material.color = color;
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
            spriteRenderer.material.color = new Color(200 / 255f, 200 / 255f, 200 / 255f);
        }

    }

    private void OnMouseUp()
    {
        spriteRenderer.material.color = new Color(245 / 255f, 245 / 255f, 245 / 255f);
    }

    private void OnMouseEnter()
    {
        if (AttackLogic.instance.canHover)
        {
            spriteRenderer.material.color = new Color(245 / 255f, 245 / 255f, 245 / 255f);
            AttackLogic.instance.showCards(presentUnits,  AttackLogic.instance.TerritoryHoverPanel);
        }
    }
    private void OnMouseExit()
    {
        if (AttackLogic.instance.canHover)
        {
            spriteRenderer.material.color = color;
            AttackLogic.instance.hideCards(presentUnits);
        }
    }

    void OnDrawGizmos()
    {
        if (territories.Count == 0) return;
        Gizmos.color = Color.red;
        foreach (Territory area in territories)
        {
            Gizmos.DrawLine(transform.position, area.transform.position);

        }
    }

}
