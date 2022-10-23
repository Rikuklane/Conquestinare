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
    private Button _button;
    public int attack;
    public int health;

    public CardInHand CardInHand = new();
    public CardInMarket CardInMarket = new();
    public CardInSelection CardInSelection = new();
    public CardInTerritory CardInTerritory = new();
    private AbstractCardState _currentState;

    public int attack;
    public int health;

    private void Awake()
    {
        if (_currentState == null)
        {
            SwitchState(CardInHand);
        }
        if (unitData != null)
        {
            SetData();
        }
    }

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
        if(attack==0)
        {
            attackText.text = unitData.attack.ToString();
        } else
        {
            attackText.text = attack.ToString();
        }
        if(health==0)
        {
            healthText.text = unitData.health.ToString();
        } else
        {
            healthText.text = health.ToString();
        }
        costText.text = unitData.cost.ToString();
        image.sprite = unitData.sprite;
    }
    
    public void SetHealth(int newHealth)
    {
        health = newHealth;
        healthText.text = health.ToString();
    }
    
    private void Selected()
    {
        StartCoroutine(_currentState.CardOnClick(this));
    }

    public void SwitchState(AbstractCardState state)
    {
        _currentState = state;
        _button = GetComponent<Button>();
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(Selected);
    }
}
