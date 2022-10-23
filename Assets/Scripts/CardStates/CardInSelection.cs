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
            UnitCardSelector.Instance.SetActive(false);
            TurnManager.Instance.TriggerEndState();
            yield break;
        }

        public override IEnumerator NextState(UnitCardPresenter card)
        {
            card.SwitchState(card.CardInHand);
            card.transform.SetParent(CardHand.Instance.transform, false);
            return base.NextState(card);
        }
    }
}