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
    private List<CardData> _cardSelection;
    private HorizontalLayoutGroup _layoutGroup;
    private readonly List<SpellCardPresenter> _selectedSpells = new();
    private readonly List<UnitCardPresenter> _selectedUnits = new();
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
        _cardSelection = new List<CardData>(CardCollection.Instance.GetSelectionOfUnits(cardsCount));
        CardSelection(CardStateController.Instance.CardInSelection);
    }
    
    private void MarketSelection(int cardsCount)
    {
        _cardSelection = CardCollection.Instance.GetSelectionOfCards(cardsCount);
        CardSelection(CardStateController.Instance.CardInMarket);
    }

    private void CardSelection(AbstractCardState cardState)
    {
        DestroyExistingChildren();
        foreach (var cardData in _cardSelection)
        {
            if (cardData.GetType() == typeof(UnitData))
            {
                CreateUnitCard(cardData as UnitData, cardState);
            }
            else if (cardData.GetType() == typeof(SpellData))
            {
                CreateSpellCard(cardData as SpellData, cardState);
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
        _selectedSpells.Clear();
        _selectedUnits.Clear();
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
        _selectedUnits.Add(unitCard);
    }
    
    private void CreateSpellCard(SpellData spellData, AbstractCardState state)
    {
        var spellCard = Instantiate(spellCardPrefab, transform.position, Quaternion.identity, transform.GetChild(0).GetChild(0));
        spellCard.cardLogic.SwitchState(state);
        spellCard.SetData(spellData);
        _selectedSpells.Add(spellCard);
    }

    public List<SpellCardPresenter> selectedSpells => _selectedSpells;
    public List<UnitCardPresenter> selectedUnits => _selectedUnits;
}
