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
            // TODO fix this triggering (currently does not let the animation finish & therefore the card does not make it to the hand)
            TurnManager.Instance.TriggerEndState();
            yield break;
        }

        public override IEnumerator NextState(UnitCardPresenter card)
        {
            card.SwitchState(card.CardInHand);
            MoveCardToHand(card);
            return base.NextState(card);
        }
    }
}