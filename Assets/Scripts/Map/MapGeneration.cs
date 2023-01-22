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


    void Start()
    {
        //GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [ContextMenu("AddBonusGroups")]
    private void AddBonusGroups()
    {
        var json = Resources.Load<TextAsset>("worldequalized").text;
        MapData mapData = JsonUtility.FromJson<MapData>(json);

        GameObject[] provinces = mapData.cells.provinces.Select(province =>
        {
            if (province.name == null) return new GameObject();
            GameObject provinceObject = new();
            GetComponent<TerritoryManager>().transform.Find("province " + province.name).GetComponent<Territory>().bonusGroup = province.state - 1;
            return provinceObject;
        }).ToArray();
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
        }).ToArray();*/


        GameObject[] provinces = mapData.cells.provinces.Select(province =>
        {
            GameObject provinceObject = new();

            provinceObject.AddComponent(typeof(MeshRenderer));
            provinceObject.AddComponent(typeof(MeshFilter));
            provinceObject.AddComponent(typeof(MeshCollider));
            provinceObject.AddComponent(typeof(ProvinceData));

            provinceObject.name = "province " + province.name;
            provinceObject.transform.parent = gameObject.transform;
            provinceObject.transform.position = new Vector3(mapData.cells.cells[province.center].p[0], mapData.cells.cells[province.center].p[1]);
            return provinceObject;
        }).ToArray();


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
                if (neighborCell.province == 0) continue;
                if (cell.province == neighborCell.province) continue;

                ProvinceData provinceData = provinces[cell.province].transform.GetComponent<ProvinceData>();
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

            province.SetActive(true);

            if (province.transform.childCount == 0) DestroyImmediate(province);
        }
    }
}
