using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MapDataModel
{

    [System.Serializable]
    public class MapData
    {
        public CellsData cells;
        public Vertex[] vertices;
    }

    [System.Serializable]
    public class Vertex
    {
        public int[] p; // vertex coordinates [x, y]
        public int[] v; // indexes of cells adjacent to vertex (3)
        public int[] c; // indexes of vertices adjacent to vertex (3)
    }


    [System.Serializable]
    public class CellsData
    {
        public Cell[] cells;
        public Feature[] features;
        public State[] states;
        public Province[] provinces;
    }

    [System.Serializable]
    public class Cell
    {
        public int i; // index
        public int[] v; // cell vertice indexes
        public int[] c; // neighboring cell indexes
        public int[] p; // cell coordinates [x, y]
        public int state; // cell state index
        public int province; // cell province index
    }

    [System.Serializable]
    public class Province
    {
        public int i; // index
        public int state; // state id
        public int center; // cell id of province center
        public string name; // short form of province name
        public string formName;
        public string fullName; // full state name.
    }

    [System.Serializable]
    public class State
    {
        public int i; // index
        public string name; // short form of state name
        public int center; // cell id of state center
        public int provinces; // id-s of state provinces
        public int fullName; // full state name
    }

    [System.Serializable]
    public class Feature
    {
        public int i; // index >= 1
        public bool land; // is height >= 20;
        public bool border; // does feature touch land border
        public string type; // ocean / island / lake
        public string group; // ocean / continent/island/isle/lake_island / freshwater/salt/dry/sinkhole/lava
        public int cells; // number of cells in feature
        public int firstCell; // index of top left cell in feature
        public int[] vertices; // perimetric vertices
    }
}
