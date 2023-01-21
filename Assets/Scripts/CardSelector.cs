using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using CardStates;
using UnityEngine;
using UnityEngine.UI;

public class CardSelector : MonoBehaviour
{
    public UnitCardPresenter unitCardPrefab;
    public SpellCardPresenter spellCardPrefab;
    public static CardSelector Instance;
    private List<UnitData> _unitSelection;
    private List<CardData> _cardSelection;
    private HorizontalLayoutGroup _layoutGroup;
    private void Awake()
    {
        Instance = this;
        _layoutGroup = gameObject.GetComponent<HorizontalLayoutGroup>();
        //gameObject.transform.DetachChildren();
        Events.OnReceiveUnitsSelection += ReceiveUnitsSelection;
        Events.OnMarketSelection += MarketSelection;
        SetActive(false);
    }

    private void OnDestroy()
    {
        Events.OnReceiveUnitsSelection -= ReceiveUnitsSelection;
        Events.OnMarketSelection -= MarketSelection;
    }

    private void ReceiveUnitsSelection(int cardsCount)
    {
        _unitSelection = CardCollection.Instance.GetSelectionOfUnits(cardsCount);
        DestroyExistingChildren();
        
        foreach (var unitData in _unitSelection)
        {
            CreateUnitCard(unitData, CardStateController.Instance.CardInSelection);
        }
        SetActive(true);
    }
    
    private void MarketSelection(int cardsCount)
    {
        _cardSelection = CardCollection.Instance.GetSelectionOfCards(cardsCount);
        DestroyExistingChildren();
        
        foreach (var cardData in _cardSelection)
        {
            if (cardData.GetType() == typeof(UnitData))
            {
                CreateUnitCard(cardData as UnitData, CardStateController.Instance.CardInMarket);
            }
        }
        foreach (var cardData in _cardSelection)
        {
            if (cardData.GetType() == typeof(SpellData))
            {
                CreateSpellCard(cardData as SpellData, CardStateController.Instance.CardInMarket);
            }
        }
        SetActive(true);
    }

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
        //_layoutGroup.gameObject.SetActive(value);
    }

    private void DestroyExistingChildren()
    {
        foreach (var child in transform.GetComponentsInChildren<UnitCardPresenter>())
        {
            Destroy(child.gameObject);
        }
        foreach (var child in transform.GetComponentsInChildren<SpellCardPresenter>())
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateUnitCard(UnitData unitData, AbstractCardState state)
    {
        var unitCard = Instantiate(unitCardPrefab, transform.position, Quaternion.identity, transform.GetChild(0).GetChild(0));
        unitCard.cardLogic.SwitchState(state);
        unitCard.SetData(unitData);
    }
    
    private void CreateSpellCard(SpellData spellData, AbstractCardState state)
    {
        var spellCard = Instantiate(spellCardPrefab, transform.position, Quaternion.identity, transform.GetChild(0).GetChild(0));
        spellCard.cardLogic.SwitchState(state);
        spellCard.SetData(spellData);
    }
}
