using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitCardPresenter : MonoBehaviour
{
    public UnitData unitData;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI costText;
    public Image image;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        if (unitData != null)
        {
            SetData();
        }
    }

    public Button Button => _button;

    public void SetData(UnitData data)
    {
        unitData = data;
        if (unitData != null)
        {
            SetData();
        }
    } 

    private void SetData()
    {
        titleText.text = unitData.title;
        descriptionText.text = unitData.description;
        attackText.text = unitData.attack.ToString();
        healthText.text = unitData.health.ToString();
        costText.text = unitData.cost.ToString();
        image.sprite = unitData.sprite;
    }

    public void SetSelectionListener(UnitCardSelector selector)
    {
        
        selector.SetActive(false);
    }
}
