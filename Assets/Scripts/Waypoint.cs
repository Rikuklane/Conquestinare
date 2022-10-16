using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public List<Waypoint> routes = new List<Waypoint>();
    private List<LineRenderer> lines = new List<LineRenderer>();

    private void Start()
    {
        CreateLines();
    }

    public List<Waypoint> GetNextWaypoints()
    {
        return routes;
    }
    void OnDrawGizmos()
    {
        if (routes.Capacity == 0) return;
        Gizmos.color = Color.red;
        foreach (Waypoint route in routes)
        {
            Gizmos.DrawLine(transform.position, route.transform.position);

        }
    }
    public void CreateLine(Waypoint lineTo)
    {
        GameObject child = new GameObject();
        child.transform.parent = gameObject.transform;
        LineRenderer lineRenderer = child.AddComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0.1f;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, lineTo.transform.position);
        lineRenderer.enabled = false;

        lines.Add(lineRenderer);
    }
    public void CreateLines()
    {
        if (lines.Capacity != 0) return;
        foreach (Waypoint route in routes)
        {
            CreateLine(route);
        }
        Debug.Log(lines.Capacity);
    }

    public void ToggleLines()
    {
        Debug.Log(lines.Capacity);
        foreach (LineRenderer line in lines)
        {
            line.enabled = !line.enabled;
        }
    }

    public void SetLine(Vector3 toPosition, bool isEnabled)
    {
        foreach (LineRenderer line in lines)
        {
            if(line.GetPosition(1) == toPosition)
            {
                line.enabled = isEnabled;

            }
        }
    }

    public void SetLines(bool isEnabled)
    {
        foreach (LineRenderer line in lines)
        {
            line.enabled = isEnabled;
        }
    }
}
