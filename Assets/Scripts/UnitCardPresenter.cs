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
    
    // TODO move these values to somewhere else where they could be used all around the code (Currently also used in CardSelector)
    public CardInHand CardInHand = new();
    public CardInMarket CardInMarket = new();
    public CardInSelection CardInSelection = new();
    public CardInTerritory CardInTerritory = new();
    private AbstractCardState _currentState;

    public bool isSelected = false;
    public bool isInteractable = true;
    private Color childColor;

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
        childColor = childObject.GetComponent<Image>().color;
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
        if (!isInteractable) return;
        Debug.Log(isSelected);
        isSelected = !isSelected;

        float alpha = 1f;
        float y = 0;
        if (isSelected)
        {
            
            y = 10;
        } else
        {
            alpha = 0.6f;
        }

        //childObject.transform.localPosition =
        LeanTween.moveLocal(childObject, new Vector3(0, y, 0), 0.2f);
        childColor.a = alpha;
        childObject.GetComponent<Image>().color = childColor;

    }

    public void changeInteractable(bool isInteract)
    {
        _button.interactable = isInteract;
        isInteractable = isInteract;
        if (isInteract)
        {
            childObject.GetComponent<Image>().color = childColor;
        }
        else
        {
            childObject.GetComponent<Image>().color = Color.red;
        }
    }

    private void OnBecameVisible()
    {
        // reset, when accidentally clicked between reorganizing
        isSelected = false;
    }

}
