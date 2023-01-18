using MapDataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapGeneration : MonoBehaviour
{
    public float scale = 1f;
    public Material MeshMaterial;

    // Start is called before the first frame update
    void Start()
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

            provinceObject.name = "province " + province.name;
            provinceObject.transform.parent = gameObject.transform;
            return provinceObject;
        }).ToArray();


        GameObject[] cells = mapData.cells.cells.Select(cell =>
        {
            Vector3[] vertices = cell.v.Select(vertID => {
                var vert = mapData.vertices[vertID];
                return new Vector3(vert.p[0], vert.p[1], 0);
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

            province.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
