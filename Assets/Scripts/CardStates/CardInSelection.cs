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
            //UnitCardSelector.Instance.SetActive(false);
            TurnManager.Instance.TriggerEndState();
            yield break;
        }

        public override IEnumerator NextState(UnitCardPresenter card)
        {
            card.SwitchState(card.CardInHand);
            LeanTween.move(card.childObject, CardHand.Instance.transform.position, 0.2f)
                .setOnComplete(()=> {
                    UnitCardSelector.Instance.SetActive(false);
                    card.transform.SetParent(CardHand.Instance.transform, false);
                    card.childObject.transform.localPosition = Vector3.zero;

                }
    );
            return base.NextState(card);
        }
    }
}