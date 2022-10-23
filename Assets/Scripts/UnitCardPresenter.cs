using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitCardPresenter : MonoBehaviour
{
    public UnitData unitData;

    public MeshRenderer titleMesh;
    public MeshRenderer descriptionMesh;
    public MeshRenderer attackMesh;
    public MeshRenderer healthMesh;
    public MeshRenderer costMesh;
    public SpriteRenderer image;

    private void Awake()
    {
        if (unitData != null)
        {
            titleMesh.GetComponent<TextMeshPro>().text = unitData.title;
            descriptionMesh.GetComponent<TextMeshPro>().text = unitData.description;
            attackMesh.GetComponent<TextMeshPro>().text = unitData.attack.ToString();
            healthMesh.GetComponent<TextMeshPro>().text = unitData.health.ToString();
            costMesh.GetComponent<TextMeshPro>().text = unitData.cost.ToString();
            image.sprite = unitData.sprite;
        }
    }
}
