using MapDataModel;
using System.Collections.Generic;
using UnityEngine;

public class ProvinceData : MonoBehaviour
{
    public List<ProvinceData> neighbors = new();
    public string provinceName = "";
    public List<int> border = new();
    public List<Vertex> borderLines = new();
}
