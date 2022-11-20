using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHand : MonoBehaviour
{
    public static CardHand Instance;
    public Dictionary<string, List<CardPresenterAbstractLogic>> cardHands = new();
    public CardPresenterAbstractLogic cardSelected;
    public UnitCardPresenter unitCardPrefab;

    public void DestroySelected()
    {
        if(cardSelected)
        {
            PlayCard(cardSelected);
            Destroy(cardSelected.CardInstance.gameObject);
            cardSelected = null;
        }
    }

    public void NewCardSelected(CardPresenterAbstractLogic cardSelect)
    {
        cardSelected = cardSelect;
        // deselect others
        foreach(CardPresenterAbstractLogic card in cardHands[Events.RequestPlayer().Name])
        {
            if (card.isSelected)
            {
                card.TriggerSelected();
            }
        }
        cardSelect.TriggerSelected();
    }

    private void Awake()
    {
        Instance = this;
    }

    public void CreateCardHands(Player[] players)
    {
        foreach (Player player in players)
        {
            cardHands[player.Name] = new();
        }

    }

    public void LoadHand(Player player)
    {
        // TODO hide all children
        foreach (Player currentPlayer in Turns.TurnManager.Instance.Players)
        {
            foreach (CardPresenterAbstractLogic card in cardHands[currentPlayer.Name])
            {
                card.CardInstance.gameObject.SetActive(false);
            }
        }

        //   - add cards from new player
        foreach(CardPresenterAbstractLogic card in cardHands[player.Name])
        {
            card.CardInstance.gameObject.SetActive(true);
        }


    }

    public void AddCard(CardPresenterAbstractLogic card)
    {
        cardHands[Events.RequestPlayer().Name].Add(card);
    }

    public void PlayCard(CardPresenterAbstractLogic card)
    {
        cardHands[Events.RequestPlayer().Name].Remove(card);
    }
}
