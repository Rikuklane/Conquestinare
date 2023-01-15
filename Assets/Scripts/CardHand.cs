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
    private Vector3 cardSelectLastPosition;

    private void Update()
    {
        if(cardSelected != null)
        {
            cardSelected.CardInstance.transform.position = Input.mousePosition;
            if (Input.GetMouseButtonDown(1))
            {
                NewCardSelected(null);
            }
        }
    }

    private void Update()
    {
        if(cardSelected != null)
        {
            print(Input.mousePosition);
            cardSelected.CardInstance.transform.position = Input.mousePosition;
        }
    }

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
        // animate last card back to hand
        if(cardSelected != null)
        {
            StartCoroutine(cardSelected.MoveBack(cardSelectLastPosition, 0.7f));
        }
        cardSelected = cardSelect;
        if (cardSelect == null)
        {
            return;
        }
        cardSelectLastPosition = cardSelect.transform.position;
        // deselect others
        foreach (CardPresenterAbstractLogic card in cardHands[Events.RequestPlayer().Name])
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
