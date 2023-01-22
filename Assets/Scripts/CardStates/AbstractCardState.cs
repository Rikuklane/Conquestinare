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
            LeanTween.move(card.childGameObject, CardHand.Instance.transform.position, 0.2f)
                .setOnComplete(()=> {
                        Debug.Log("[AbstractCardState] Card moved to hand");
                        card.cardInstance.transform.SetParent(CardHand.Instance.transform.GetChild(0).GetChild(0), false);
                        card.childGameObject.transform.localPosition = Vector3.zero;
                        card.FadeCard();
                        if (endTurn) TurnManager.Instance.TriggerTurnEndStateButton();
                    }
                );
        }
    }
}
