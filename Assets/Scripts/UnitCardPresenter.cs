using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitCardPresenter : MonoBehaviour
{
    public UnitData unitData;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI costText;

    private void Awake()
    {
        if (unitData != null)
        {
            titleText.text = unitData.title;
            descriptionText.text = unitData.description;
            attackText.text = unitData.attack.ToString();
            healthText.text = unitData.health.ToString();
            costText.text = unitData.cost.ToString();
        }
    }
}
