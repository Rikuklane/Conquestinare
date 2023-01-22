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
    public float speed = 5f;
    public ParticleSystem particleSystem;
    private Vector3 cardSelectLastPosition;
    private int currentNumberParticles;

    private void Update()
    {
        if(cardSelected != null)
        {
            //particleSystem.Stop();
            cardSelected.cardInstance.transform.position = Vector3.Lerp(cardSelected.cardInstance.transform.position, Input.mousePosition, Time.deltaTime * speed);
            particleSystem.transform.position = Camera.main.ScreenToWorldPoint(cardSelected.cardInstance.transform.position);
            if(particleSystem.particleCount > currentNumberParticles)
            {
                AudioController.Instance.sparkle.Play();
            }
            currentNumberParticles = particleSystem.particleCount;
            //particleSystem.Play();
            //cardSelected.CardInstance.transform.position = Input.mousePosition;
            if (Input.GetMouseButtonDown(1))
            {
                NewCardSelected(null);
            }
        }
    }

    public void DestroySelected()
    {
        if(cardSelected)
        {
            PlayCard(cardSelected);
            particleSystem.transform.parent = transform;
            Destroy(cardSelected.cardInstance.gameObject);
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
        particleSystem.transform.parent = cardSelected.cardInstance.transform;
        particleSystem.transform.position = cardSelected.cardInstance.transform.position;

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
        cardSelect.cardInstance.transform.parent = transform.parent;
    }

    private void Awake()
    {
        Instance = this;
        particleSystem = Instantiate(particleSystem, transform);
    }

    public void CreateCardHands(Player[] players)
    {
        foreach (Player player in players)
        {
            cardHands[player.Name] = new();
        }

    }

    public void HideCurrentHand()
    {
        // TODO hide all children
        foreach (Player currentPlayer in Turns.TurnManager.Instance.Players)
        {
            foreach (CardPresenterAbstractLogic card in cardHands[currentPlayer.Name])
            {
                card.cardInstance.gameObject.SetActive(false);
            }
        }
    }

    public void LoadHand(Player player)
    {
        //   - add cards from new player
        foreach(CardPresenterAbstractLogic card in cardHands[player.Name])
        {
            card.cardInstance.gameObject.SetActive(true);
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
