using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Territory : MonoBehaviour
{
    public Waypoint waypoint;
    public Color color;

    public List<Territory> territories = new List<Territory>();
    public List<Vector3> enemyTerritories = new List<Vector3>();
    public bool isEnemy;

    public List<UnitCardPresenter> startUnits = new List<UnitCardPresenter>();
    public List<UnitCardPresenter> presentUnits = new List<UnitCardPresenter>();
    private List<Unit> units = new List<Unit>();


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

        int attack = 0;
        int health = 0;
        foreach (UnitCardPresenter unit in startUnits)
        {

            attack += unit.unitData.attack;
            health += unit.unitData.health;
        }
        summaryText.text = attack.ToString() + "AD/" + health.ToString() + "HP";

        List<Unit> newUnits = new List<Unit>();
        foreach (UnitCardPresenter unit in startUnits)

        {
            UnitCardPresenter card = GameObject.Instantiate<UnitCardPresenter>(unit, AttackLogic.instance.TerritoryHoverPanel.transform);
            card.gameObject.SetActive(false);
            presentUnits.Add(card);

            Unit newUnit = new Unit();
            newUnit.attack = unit.unitData.attack;
            newUnit.health = unit.unitData.health;
            newUnits.Add(newUnit);

        }
        units = newUnits;

    }

    public List<Unit> GetUnits()
    {
        return units;
    }

    public void AddCard(UnitCardPresenter unit)
    {
        UnitCardPresenter card = GameObject.Instantiate<UnitCardPresenter>(unit, AttackLogic.instance.TerritoryHoverPanel.transform);
        card.gameObject.SetActive(false);
        presentUnits.Add(card);
        Unit newUnit = new Unit();
        newUnit.attack = unit.unitData.attack;
        newUnit.health = unit.unitData.health;
        units.Add(newUnit);
        SetSummary();
        //TODO
    }

    public void SetSummary()
    {
        int attack = 0;
        int health = 0;
        foreach(Unit unit in units)
        {
            attack += unit.attack;
            health += unit.health;
        }
        summaryText.text = attack.ToString() + "AD/" + health.ToString() + "HP";
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
            if (isEnemy != area.isEnemy)
            {
                enemyTerritories.Add(area.waypoint.transform.position);
            }
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AttackLogic.instance.SelectTerritory(this);
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
        spriteRenderer.material.color = new Color(245 / 255f, 245 / 255f, 245 / 255f);
        AttackLogic.instance.showCards(presentUnits,  AttackLogic.instance.TerritoryHoverPanel);

    }
    private void OnMouseExit()
    {
        spriteRenderer.material.color = color;
        AttackLogic.instance.hideCards(presentUnits);
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
