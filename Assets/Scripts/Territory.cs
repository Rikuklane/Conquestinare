using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Territory : MonoBehaviour
{
    public Waypoint waypoint;
    public Color color;
    public List<Territory> territories = new List<Territory>();
    public List<Vector3> enemyTerritories = new List<Vector3>();
    public bool isEnemy;
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

        Debug.Log(enemyTerritories);


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
            Game.instance.SelectTerritory(this);
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

    }
    private void OnMouseExit()
    {
        spriteRenderer.material.color = color;
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
