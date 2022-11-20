using System;
using System.Collections;
using System.Collections.Generic;
using CardStates;
using TMPro;
using Unity.VisualScripting;
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
    public GameObject childObject;
    private Button _button;
    public int attack;
    public int health;
    public CardPresenterAbstractLogic CardLogic;

    private void Awake()
    {
        if (unitData != null)
        {
            SetData();
        }
        CardLogic = new CardPresenterAbstractLogic(gameObject, childObject, unitData);
    }

    public void SetData(UnitData data)
    {
        unitData = data;
        CardLogic.CardData = unitData;
        if (unitData != null)
        {
            SetData();
        }
    }

    private void SetData()
    {
        titleText.text = unitData.title;
        descriptionText.text = unitData.description;
        attackText.text = attack==0 ? unitData.attack.ToString() : attack.ToString();
        healthText.text = health==0 ? unitData.health.ToString() : health.ToString();
        costText.text = unitData.cost.ToString();
        image.sprite = unitData.sprite;
    }
    
    public void SetHealth(int newHealth)
    {
        health = newHealth;
        healthText.text = health.ToString();
    }
}
