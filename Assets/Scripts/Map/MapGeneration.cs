using MapDataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class MapGeneration : MonoBehaviour
{
    public float scale = 1f;
    public Material MeshMaterial;

    private Color[] stateColors =
    {
        new Color32(0, 0, 0, 255),
        new Color32(5, 235, 143, 255),
        new Color32(235, 52, 119, 255),
        new Color32(79, 52, 235, 255),
        new Color32(220, 235, 52, 255),
        new Color32(52, 201, 235, 255),
        new Color32(52, 235, 79, 255),
    };

    void Start()
    {
        //GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [ContextMenu("Generate")]
    private void GenerateMap()
    {
        var json = Resources.Load<TextAsset>("worldequalized").text;
        MapData mapData = JsonUtility.FromJson<MapData>(json);

        /* Test Vertices
        GameObject[] vertexSpheres = mapData.vertices.Select((vert, i) => {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = new Vector3(vert.p[0], vert.p[1], 0);
            sphere.transform.localScale = new Vector3(scale, scale, scale);
            sphere.transform.parent = gameObject.transform;
            sphere.name = "vertex" + i;
            return sphere;
        }).ToArray();
        */

        GameObject[] provinces = mapData.cells.provinces.Select(province =>
        {
            GameObject provinceObject = new();

            provinceObject.AddComponent(typeof(MeshRenderer));
            provinceObject.AddComponent(typeof(MeshFilter));
            provinceObject.AddComponent(typeof(MeshCollider));
            provinceObject.AddComponent(typeof(ProvinceData));
            provinceObject.AddComponent(typeof(OutlineCreator));
            provinceObject.GetComponent<OutlineCreator>().material = MeshMaterial;
            provinceObject.GetComponent<OutlineCreator>().color = stateColors[province.state];
            provinceObject.GetComponent<ProvinceData>().provinceName = province.fullName;
            provinceObject.GetComponent<ProvinceData>().bonus = province.state;

            provinceObject.name = "province " + province.name;
            provinceObject.transform.parent = gameObject.transform;
            provinceObject.transform.position = new Vector3(mapData.cells.cells[province.center].p[0], mapData.cells.cells[province.center].p[1]);
            return provinceObject;
        }).ToArray();

        /*
        GameObject[] states = mapData.cells.states.Select(state =>
        {
            GameObject stateObject = new();

            stateObject.AddComponent(typeof(OutlineCreator));
            stateObject.AddComponent(typeof(ProvinceData));
            stateObject.GetComponent<OutlineCreator>().lineWidth = 10f;
            stateObject.GetComponent<OutlineCreator>().material = MeshMaterial;
            stateObject.GetComponent<ProvinceData>().provinceName = state.name;

            stateObject.name = "state " + state.name;
            stateObject.transform.parent = gameObject.transform;
            stateObject.transform.position = new Vector3(mapData.cells.cells[state.center].p[0], mapData.cells.cells[state.center].p[1]);

            return stateObject;
        }).ToArray();
        */

        GameObject[] cells = mapData.cells.cells.Select(cell =>
        {

            Vector3[] vertices = cell.v.Select(vertID => {
                var vert = mapData.vertices[vertID];
                int[] provincePos = mapData.cells.cells[mapData.cells.provinces[cell.province].center].p;
                return new Vector3(vert.p[0]-cell.p[0]- provincePos[0], vert.p[1]-cell.p[1]- provincePos[1], 0);
            }).ToArray();

            int[] indices = new int[(vertices.Length - 2) * 3];
            for (int i = 2; i < vertices.Length; i++)
            {
                int index = i - 2;
                indices[index * 3] = i;
                indices[index * 3 + 1] = i - 1;
                indices[index * 3 + 2] = 0;
            }

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = indices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            GameObject cellObject = new();
            cellObject.name = cell.i.ToString();
            MeshRenderer renderer = cellObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
            MeshFilter filter = cellObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
            filter.mesh = mesh;

            cellObject.transform.parent = provinces[cell.province].transform;
            cellObject.transform.position = new Vector3(cell.p[0], cell.p[1], 0);

            if (cell.province == 0) return cellObject;

            foreach (int neighborCellIndex in cell.c)
            {
                Cell neighborCell = mapData.cells.cells[neighborCellIndex];
                if (cell.province == neighborCell.province) continue;


                ProvinceData provinceData = provinces[cell.province].transform.GetComponent<ProvinceData>();
                // ProvinceData stateData = states[cell.state].transform.GetComponent<ProvinceData>();
                foreach (int vertexIndex in cell.v)
                {
                    Vertex vertex = mapData.vertices[vertexIndex];
                    if (IsBorder(mapData, vertex, cell.province))
                    {
                        provinceData.border.Add(vertexIndex);
                    }
                    /*
                    if (IsStateBorder(mapData, vertex, cell.state))
                    {
                        stateData.border.Add(vertexIndex);
                    }
                    */
                }

                if (neighborCell.province == 0) continue;
                
                ProvinceData neighborProvinceData = provinces[neighborCell.province].transform.GetComponent<ProvinceData>();
                provinceData.neighbors.Add(neighborProvinceData);
            }

            return cellObject;
        }).ToArray();

        foreach (GameObject province in provinces)
        {
            MeshFilter[] meshFilters = province.GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            for (int i = 0; i < meshFilters.Length; i++)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);
            }

            combine = combine.Skip(1).ToArray(); // Remove Warnings

            province.GetComponent<MeshFilter>().mesh = new Mesh();
            province.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);

            MeshRenderer renderer = province.GetComponent<MeshRenderer>();

            renderer.material = MeshMaterial;
            renderer.material.color = Random.ColorHSV();

            MeshCollider collider = province.GetComponent<MeshCollider>();

            collider.sharedMesh = province.GetComponent<MeshFilter>().mesh;

            ProvinceData provinceData = province.GetComponent<ProvinceData>();
            provinceData.neighbors = provinceData.neighbors.Distinct().ToList();
            provinceData.border = provinceData.border.Distinct().ToList();
            provinceData.borderLines = OrderVertices(mapData, provinceData.border);

            province.SetActive(true);

            if (province.transform.childCount == 0) DestroyImmediate(province);
        }
        /*
        foreach (GameObject state in states)
        {
            ProvinceData stateData = state.GetComponent<ProvinceData>();
            stateData.border = stateData.border.Distinct().ToList();
            // stateData.borderLines = OrderVertices(mapData, stateData.border);

            state.SetActive(true);
        }
        */
    }

    private bool IsBorder(MapData mapData, Vertex vertex, int provinceIndex)
    {
        Cell[] cells = mapData.cells.cells;

        int provinceCells = 0;

        foreach (int cellIndex in vertex.c)
        {
            if (cells[cellIndex].province == provinceIndex) provinceCells++;
        }

        if (provinceCells == 0 || provinceCells == 3) return false;
        return true;
    }

    /*
    private bool IsStateBorder(MapData mapData, Vertex vertex, int stateIndex)
    {
        Cell[] cells = mapData.cells.cells;

        int stateCells = 0;

        foreach (int cellIndex in vertex.c)
        {
            if (cells[cellIndex].state == stateIndex) stateCells++;
        }

        if (stateCells == 0 || stateCells == 3) return false;
        return true;
    }
    */

    private List<Vertex> OrderVertices(MapData mapData, List<int> vOriginal)
    {
        List<int> vCopy = new(vOriginal);
        List<Vertex> borders = new();

        Vertex[] vertices = mapData.vertices;

        while (vCopy.Count() > 0)
        {
            Debug.Log("Hey");
            List<int> cycle = new();
            cycle.Add(vCopy[0]);
            vCopy.RemoveAt(0);

            cycle = LongestCycle(vertices, vOriginal, cycle);
            cycle.ForEach(vId => vCopy.Remove(vId));

            var cycleVertices = cycle.Select(vert => {
                return vertices[vert];
            }).ToList();

            borders.AddRange(cycleVertices);
        }

        return borders;
    }

    private List<int> LongestCycle(Vertex[] vertices, List<int> vBorder, List<int> cycle)
    {   
        if (cycle.Count() > 3 && cycle.Last() == cycle.First()) return cycle;
        if (cycle.Count() > 1 && !vBorder.Contains(cycle.Last())) return new();

        Vertex vertex = vertices[cycle.Last()];
        List<int> currentLongest = new();
        
        foreach (int vNext in vertex.v) {
            if (cycle.Contains(vNext) && cycle.First() != vNext) continue;

            List<int> cycleWithVNext = new(cycle);
            cycleWithVNext.Add(vNext);

            List<int> nextCycle = LongestCycle(vertices, vBorder, cycleWithVNext);

            if (nextCycle.Count() > currentLongest.Count()) currentLongest = nextCycle;
        }

        return currentLongest;
    }


    /*
    private Vertex GetFirstBorderVertex(MapData mapData, Vertex current, int provinceIndex)
    {
        Queue<Vertex> queue = new();

        queue.Enqueue(current);

        while (true)
        {
            int queueLength = queue.Count();
            for (int i = 0; i < queueLength; i++)
            {
                Vertex vertex = queue.Dequeue();
                if (IsBorder(mapData, vertex, provinceIndex)) return vertex;
                foreach (int nextVertex in vertex.v)
                {
                    queue.Enqueue(mapData.vertices[nextVertex]);
                }
            }
        }
    }*/

    /*
    private List<Vertex> GetBorderVertices(MapData mapData, int middleCellIndex)
    {
        Vertex[] vertices = mapData.vertices;
        Cell[] cells = mapData.cells.cells;

        int provinceIndex = cells[middleCellIndex].province;

        // Find first vertex
        Vertex middle = vertices[cells[middleCellIndex].v[0]];

        // Find border vertex
        Vertex first = GetFirstBorderVertex(mapData, middle, provinceIndex);

        List<Vertex> border = new();
        // Find all border vertices

        Queue<Vertex> queue = new();
        queue.Enqueue(first);

        while (queue.Count() != 0)
        {
            int queueLength = queue.Count();
            for (int i = 0; i < queueLength; i++)
            {
                Vertex vertex = queue.Dequeue();
                if (IsBorder(mapData, vertex, provinceIndex))
                {
                    border.Add(vertex);
                    foreach (int nextVertex in vertex.v)
                    {
                        queue.Enqueue(vertices[nextVertex]);
                    }
                }
            }
        }
        // Return when back to first border vertex

        return null;
    }*/
}
