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
    public AnimationCurve animationCurve;

    private readonly Color _notInteractableColor = Color.gray;
    private Color _defaultColor;
    private AbstractCardState _currentState;

    public void SetVariables(GameObject cardInstance, GameObject child, CardData cardData)
    {
        this.cardInstance = cardInstance;
        childGameObject = child;
        _defaultColor = childGameObject.GetComponent<Image>().color;
        this.cardData = cardData;
        if (_currentState == null)
        {
            SwitchState(CardStateController.Instance.CardInHand);
        }
    }

    public void ChangeInteractable(bool isInteract)
    {
        cardButton.interactable = isInteract;
        isInteractable = isInteract;
        childGameObject.GetComponent<Image>().color = isInteract ? _defaultColor : _notInteractableColor;
    }

    public void SwitchState(AbstractCardState state)
    {
        _currentState = state;
        cardButton.onClick.RemoveAllListeners();
        cardButton.onClick.AddListener(SelectCard);
    }

    public void SelectCard()
    {
        StartCoroutine(_currentState.CardOnClick(this));
    }

    public void TriggerSelected()
    {
        if (!isInteractable) return;
        isSelected = !isSelected;

        float alpha = 1f;
        float y = 0;
        if (isSelected)
        {
            y = 10;
            if(_currentState.GetType() == typeof(CardInHand))
            {
                LeanTween.scale(cardInstance, new Vector3(0.5f, 0.5f, 0.5f), 0.25f);
                CanvasGroup canvasGroup = cardButton.gameObject.AddComponent<CanvasGroup>();
                canvasGroup.blocksRaycasts = false;
            }
        } else
        {
            alpha = 0.6f;
            if (cardInstance.GetComponent<CanvasGroup>() != null)
            {
                LeanTween.scale(cardInstance, new Vector3(1f, 1f, 1f), 0.25f);
                Destroy(cardInstance.GetComponent<CanvasGroup>());
            }
        }

        LeanTween.moveLocal(childGameObject, new Vector3(0, y, 0), 0.2f);
        _defaultColor.a = alpha;

        childGameObject.GetComponent<Image>().color = _defaultColor;
    }
    public IEnumerator MoveBack(Vector3 target, float duration)
    {
        //animationTo.transform.SetParent(transform, true);
        Vector3 origin = cardInstance.transform.position;
        float timePassed = 0f;
        while (timePassed <= duration)
        {
            timePassed += Time.deltaTime;
            float percent = Mathf.Clamp01(timePassed / duration);
            float curvePercent = animationCurve.Evaluate(percent);
            cardInstance.transform.position = Vector3.LerpUnclamped(origin, target, curvePercent);
            yield return null;
        }
        if(isSelected)
        {
            TriggerSelected();
        }
        cardInstance.transform.parent = CardHand.Instance.transform.GetChild(0).GetChild(0).transform;
    }

    public void FadeCard()
    {
        _defaultColor.a = 0.6f;
        childGameObject.GetComponent<Image>().color = _defaultColor;
    }

    public GameObject childGameObject { get; private set;}
    public GameObject cardInstance { get; private set; }
    public CardData cardData { get; set; }

    private void OnBecameVisible()
    {
        // reset, when accidentally clicked between reorganizing
        isSelected = false;
    }
}
