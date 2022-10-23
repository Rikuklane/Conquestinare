using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public List<Waypoint> routes = new();
    private List<LineRenderer> lines = new();

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
        lineRenderer.material.color = Color.white;
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

    public void SetLine(Vector3 toPosition, bool isEnabled)
    {
        foreach (LineRenderer line in lines)
        {
            if(line.GetPosition(1) == toPosition)
            {

                line.widthMultiplier = 0.3f;

                line.enabled = isEnabled;

            }
        }
    }

    public void SetLines(List<Vector3> targets, bool isEnabled)
    {
        foreach (LineRenderer line in lines)
        {
            if(targets.Contains(line.GetPosition(1)))
            {
                line.enabled = isEnabled;
            }
            line.widthMultiplier = 0.1f;

        }
    }
}
