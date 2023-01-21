using System.Collections;
using System.Collections.Generic;
using Turns;
using UnityEngine;

namespace CardStates
{
    public abstract class AbstractCardState
    {
        public virtual IEnumerator CardOnClick(CardPresenterAbstractLogic card)
        {
            yield break;
        }

        public virtual IEnumerator NextState(CardPresenterAbstractLogic card)
        {
            yield break;
        }

        protected void MoveCardToHand(CardPresenterAbstractLogic card, bool endTurn)
        {
            CardHand.Instance.AddCard(card);
            LeanTween.move(card.ChildGameObject, CardHand.Instance.transform.position, 0.2f)
                .setOnComplete(()=> {
                        card.CardInstance.transform.SetParent(CardHand.Instance.transform.GetChild(0).GetChild(0), false);
                        card.ChildGameObject.transform.localPosition = Vector3.zero;
                        card.FadeCard();
                        if (endTurn) TurnManager.Instance.TriggerEndStateButton();
                    }
                );
        }
    }
}
