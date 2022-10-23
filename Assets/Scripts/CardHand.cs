using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHand : MonoBehaviour
{
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

    private void Awake()
    {
        Instance = this;
    }

    public void LoadHand(Player player)
    {
        // TODO delete all children
        //   - add cards from new player
        
    }

    public void AddCard(Player player)
    {
        
    }

    public void PlayCard(Player player)
    {
        
    }
}
