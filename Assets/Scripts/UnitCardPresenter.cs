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

    public CardInHand CardInHand = new();
    public CardInMarket CardInMarket = new();
    public CardInSelection CardInSelection = new();
    public CardInTerritory CardInTerritory = new();
    public CardInTerritory CardInReorganize = new();
    private AbstractCardState _currentState;

    public bool isSelected = false;

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

    public void TriggerSelected()
    {
        Debug.Log(isSelected);
        isSelected = !isSelected;

        //float alpha = 1f;
        float y = 0;
        if (isSelected)
        {
            //  alpha = 0.6f;
            y = 10;
        }

        //childObject.transform.localPosition =
        LeanTween.moveLocal(childObject, new Vector3(0, y, 0), 0.2f);

        Color color = childObject.GetComponent<Image>().color;
        color.a = 1;
        childObject.GetComponent<Image>().color = color;

    }

    private void OnBecameVisible()
    {
        // reset, when accidentally clicked between reorganizing
        isSelected = false;
    }

}
