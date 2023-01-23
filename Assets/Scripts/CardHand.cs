using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CardHand : MonoBehaviour
{
    public static CardHand Instance;
    public Dictionary<string, List<CardPresenterAbstractLogic>> cardHands = new();
    public CardPresenterAbstractLogic cardSelected;
    public UnitCardPresenter unitCardPrefab;
    public float speed = 5f;
    [FormerlySerializedAs("particleSystem")] public ParticleSystem particleSystemHand;
    private Vector3 cardSelectLastPosition;
    private int currentNumberParticles;

    private void Update()
    {
        if(cardSelected != null)
        {
            //particleSystem.Stop();
            cardSelected.cardInstance.transform.position = Vector3.Lerp(cardSelected.cardInstance.transform.position, Input.mousePosition, Time.deltaTime * speed);
            particleSystemHand.transform.position = Camera.main.ScreenToWorldPoint(cardSelected.cardInstance.transform.position);
            if(particleSystemHand.particleCount > currentNumberParticles)
            {
                AudioController.Instance.sparkle.Play();
            }
            currentNumberParticles = particleSystemHand.particleCount;
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
            particleSystemHand.transform.parent = transform;
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
        particleSystemHand.transform.parent = cardSelected.cardInstance.transform;
        particleSystemHand.transform.position = cardSelected.cardInstance.transform.position;

        cardSelectLastPosition = cardSelect.transform.position;
        // deselect others
        foreach (CardPresenterAbstractLogic card in cardHands[Events.RequestPlayer().name])
        {
            if (card.isSelected)
            {
                card.TriggerSelected();
            }
        }
        cardSelect.TriggerSelected();
        cardSelect.cardInstance.transform.SetParent(transform.parent);
    }

    private void Awake()
    {
        Instance = this;
        particleSystemHand = Instantiate(particleSystemHand, transform);
    }

    public void CreateCardHands(Player[] players)
    {
        foreach (Player player in players)
        {
            cardHands[player.name] = new();
        }

    }

    public void HideCurrentHand()
    {
        // TODO hide all children
        foreach (Player currentPlayer in Turns.TurnManager.Instance.Players)
        {
            foreach (CardPresenterAbstractLogic card in cardHands[currentPlayer.name])
            {
                card.cardInstance.gameObject.SetActive(false);
            }
        }
    }

    public void LoadHand(Player player)
    {
        //   - add cards from new player
        foreach(CardPresenterAbstractLogic card in cardHands[player.name])
        {
            card.cardInstance.gameObject.SetActive(true);
        }
    }

    public void AddCard(CardPresenterAbstractLogic card)
    {
        cardHands[Events.RequestPlayer().name].Add(card);
    }

    public void PlayCard(CardPresenterAbstractLogic card)
    {
        cardHands[Events.RequestPlayer().name].Remove(card);
    }
}
