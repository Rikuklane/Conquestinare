using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHand : MonoBehaviour
{
    public List<UnitCardPresenter> cards = new();
    public static CardHand Instance;
    public UnitCardPresenter cardSelected;

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
        foreach(UnitCardPresenter card in cards)
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

    public void LoadHand(Player player)
    {
        // TODO delete all children
        //   - add cards from new player
        
    }

    public void AddCard(UnitCardPresenter card)
    {
        cards.Add(card);
    }

    public void PlayCard(Player player)
    {
        
    }
}
