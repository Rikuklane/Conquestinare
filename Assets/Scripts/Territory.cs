using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Territory : MonoBehaviour
{
    // Start is called before the first frame update
    public Waypoint waypoint;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Game.instance.newSelected(this);
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
        spriteRenderer.material.color = Color.white;
    }

}
