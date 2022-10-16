using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game instance;
    public Territory selectedFrom;
    public Territory selectedTo;
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
    public void newSelected(Territory newSelected)
    {
        if(selectedFrom == null)
        {
            selectedFrom = newSelected;
            selectedFrom.waypoint.SetLines(true);
            return;

        }
        if(selectedFrom.waypoint.routes.Contains(newSelected.waypoint))
        {
            Debug.Log("Contains");

            selectedTo = newSelected;
            // clear old lines
            selectedFrom.waypoint.SetLines(false);
            selectedFrom.waypoint.SetLine(selectedTo.waypoint.transform.position, true);
        } else
        {
            Debug.Log("Contains not");

            // clear old lines
            selectedFrom.waypoint.SetLines(false);
            newSelected.waypoint.SetLines(true);
            selectedFrom = newSelected;
        }
    }
}
