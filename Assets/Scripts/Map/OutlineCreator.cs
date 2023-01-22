using MapDataModel;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OutlineCreator : MonoBehaviour
{
    public float lineWidth = 5;
    public Material material;

    void Start()
    {
        // Stop if no province data exists
        if (GetComponent<ProvinceData>() == null)
        {
            return;
        }

        List<Vertex> borderLines = GetComponent<ProvinceData>().borderLines;

        List<List<Vertex>> bordersData = new();

        if (borderLines.Count == 0) return; 
        Vertex lastFirst = borderLines.First();
        List<Vertex> borderData = new();

        for (int i = 0; i < borderLines.Count(); i++)
        {
            if (borderData.Count() == 0)
            {
                lastFirst = borderLines[i];
                borderData.Add(lastFirst);
                continue;
            }

            if (borderData.Count() > 3 && borderLines[i].p[0] == lastFirst.p[0] && borderLines[i].p[1] == lastFirst.p[1])
            {
                borderData.Add(borderLines[i]);
                bordersData.Add(borderData);
                borderData = new();
                continue;
            }
            borderData.Add(borderLines[i]);
        }



        GameObject borders = new();
        borders.transform.name = "Borders";
        borders.transform.parent = gameObject.transform;

        // Create line prefab
        LineRenderer linePrefab = new GameObject().AddComponent<LineRenderer>();
        linePrefab.transform.name = "Border " + GetComponent<ProvinceData>().provinceName;
        linePrefab.positionCount = 0;
        linePrefab.material = material;
        linePrefab.startWidth = linePrefab.endWidth = lineWidth;

        List<GameObject> lines = new();

        foreach (List<Vertex> bData in bordersData)
        {

            GameObject lineObject = Instantiate(linePrefab.gameObject);
            lineObject.transform.parent = borders.transform;

            lines.Add(lineObject);

            LineRenderer line = lineObject.GetComponent<LineRenderer>();

            var points = bData.Select(vert => {
                return new Vector3(vert.p[0], vert.p[1], -0.01f);
            }).ToArray();

            line.positionCount = points.Count();
            line.SetPositions(points);
        }
    }
}