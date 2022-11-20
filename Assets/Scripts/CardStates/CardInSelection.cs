using System;
using System.Collections;
using Turns;
using Unity.VisualScripting;
using UnityEngine;

namespace CardStates
{
    public class CardInSelection: AbstractCardState
    {

        public override IEnumerator CardOnClick(UnitCardPresenter card)
        {
            NextState(card);
            yield break;
        }

        public override IEnumerator NextState(UnitCardPresenter card)
        {
            card.SwitchState(card.CardInHand);
            MoveCardToHand(card, true);
            return base.NextState(card);
        }
    }
}