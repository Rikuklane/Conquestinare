using System;
using System.Collections;
using Turns;
using Unity.VisualScripting;
using UnityEngine;

namespace CardStates
{
    public class CardInSelection: AbstractCardState
    {

        public override IEnumerator CardOnClick(CardPresenterAbstractLogic card)
        {
            Debug.Log(card);
            AudioController.Instance.cardChoiceSelect.Play();
            NextState(card);
            yield break;
        }

        public override IEnumerator NextState(CardPresenterAbstractLogic card)
        {
            card.SwitchState(CardStateController.Instance.CardInHand);
            MoveCardToHand(card, true);
            return base.NextState(card);
        }
    }
}
