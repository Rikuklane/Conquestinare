using System.Collections;
using System.Collections.Generic;
using CardStates;
using UnityEngine;
using UnityEngine.UI;

public class CardPresenterAbstractLogic
{
    public bool isSelected;
    public bool isInteractable = true;
    
    private readonly Color _notInteractableColor = Color.gray;
    private Color _defaultColor;
    private AbstractCardState _currentState;
    private readonly Button _cardButton;

    public CardPresenterAbstractLogic(GameObject cardInstance, GameObject child, CardData cardData)
    {
        CardInstance = cardInstance;
        ChildGameObject = child;
        _cardButton = CardInstance.GetComponent<Button>();
        _defaultColor = ChildGameObject.GetComponent<Image>().color;
        CardData = cardData;
        SwitchState(CardStateController.Instance.CardInHand);
    }
    
    public void ChangeInteractable(bool isInteract)
    {
        _cardButton.interactable = isInteract;
        isInteractable = isInteract;
        ChildGameObject.GetComponent<Image>().color = isInteract ? _defaultColor : _notInteractableColor;
    }
    
    public void SwitchState(AbstractCardState state)
    {
        _currentState = state;
        _cardButton.onClick.RemoveAllListeners();
        _cardButton.onClick.AddListener(Selected);
    }
    
    private void OnBecameVisible()
    {
        // reset, when accidentally clicked between reorganizing
        isSelected = false;
    }
    
    private void Selected()
    {
       _currentState.CardOnClick(this);
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

    public GameObject ChildGameObject { get; }
    public GameObject CardInstance { get; }
    public CardData CardData { get; set; }
}
