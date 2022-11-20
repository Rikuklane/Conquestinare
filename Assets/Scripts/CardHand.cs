using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHand : MonoBehaviour
{
    public static CardHand Instance;
    public Dictionary<string, List<UnitCardPresenter>> cardHands = new();
    public UnitCardPresenter cardSelected;
    public UnitCardPresenter cardPrefab;

    public void DestroySelected()
    {
        if(cardSelected)
        {
            Destroy(cardSelected.gameObject);
            cardSelected = null;
        }
    }

    public void NewCardSelected(UnitCardPresenter cardSelect)
    {
        cardSelected = cardSelect;
        // deselect others
        foreach(UnitCardPresenter card in cardHands[Events.RequestPlayer().Name])
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
            foreach (UnitCardPresenter card in cardHands[currentPlayer.Name])
            {
                card.gameObject.SetActive(false);
            }
        }

        //   - add cards from new player
        foreach(UnitCardPresenter card in cardHands[player.Name])
        {
            card.gameObject.SetActive(true);
        }


    }

    public void AddCard(UnitCardPresenter card)
    {
        cardHands[Events.RequestPlayer().Name].Add(card);
    }

    public void PlayCard(Player player)
    {
        
    }
}
