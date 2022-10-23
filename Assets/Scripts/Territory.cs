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

    public GameObject cardPrefab;
    public List<Unit> units = new List<Unit>();

    public GameObject hoverPanel;
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

        setSummary();

        Debug.Log(enemyTerritories);


    }

    public void addCard(CardData card)
    {
        //TODO
    }

    public void setSummary()
    {
        int attack = 0;
        int health = 0;
        foreach(Unit unit in units)
        {
            attack += unit.unitData.attack;
            health += unit.unitData.health;
        }
        summaryText.text = attack.ToString() + "AD/" + health.ToString() + "HP";
    }

    public void setColor(Color _color)
    {
        color = _color;
        spriteRenderer.material.color = color;
    }

    public void showAttackOptions()
    {
        waypoint.SetLines(enemyTerritories, true);
    }

    public void hideAttackOptions()
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
        AttackLogic.instance.showCards(units, cardPrefab);

    }
    private void OnMouseExit()
    {
        spriteRenderer.material.color = color;
        AttackLogic.instance.hideCards();
    }

    void OnDrawGizmos()
    {
        if (territories.Capacity == 0) return;
        Gizmos.color = Color.red;
        foreach (Territory area in territories)
        {
            Gizmos.DrawLine(transform.position, area.transform.position);

        }
    }

}
