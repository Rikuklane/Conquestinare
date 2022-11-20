using System;
using System.Collections;
using System.Collections.Generic;
using CardStates;
using UnityEngine;
using UnityEngine.UI;

public class CardPresenterAbstractLogic: MonoBehaviour
{
    public bool isSelected;
    public bool isInteractable = true;
    public Button cardButton;
    
    private readonly Color _notInteractableColor = Color.gray;
    private Color _defaultColor;
    private AbstractCardState _currentState;

    public void SetVariables(GameObject cardInstance, GameObject child, CardData cardData)
    {
        CardInstance = cardInstance;
        ChildGameObject = child;
        _defaultColor = ChildGameObject.GetComponent<Image>().color;
        CardData = cardData;
        SwitchState(CardStateController.Instance.CardInHand);
    }
    
    public void ChangeInteractable(bool isInteract)
    {
        cardButton.interactable = isInteract;
        isInteractable = isInteract;
        ChildGameObject.GetComponent<Image>().color = isInteract ? _defaultColor : _notInteractableColor;
    }
    
    public void SwitchState(AbstractCardState state)
    {
        _currentState = state;
        cardButton.onClick.RemoveAllListeners();
        cardButton.onClick.AddListener(Selected);
    }

    private void Selected()
    {
       StartCoroutine(_currentState.CardOnClick(this));
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
        
        LeanTween.moveLocal(ChildGameObject, new Vector3(0, y, 0), 0.2f);
        _defaultColor.a = alpha;
        ChildGameObject.GetComponent<Image>().color = _defaultColor;
    }

    public GameObject ChildGameObject { get; private set;}
    public GameObject CardInstance { get; private set; }
    public CardData CardData { get; set; }
    
    private void OnBecameVisible()
    {
        // reset, when accidentally clicked between reorganizing
        isSelected = false;
    }
}
