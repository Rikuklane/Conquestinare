using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardStates
{
    public abstract class AbstractCardState
    {
        public virtual IEnumerator CardOnClick(UnitCardPresenter card)
        {
            yield break;
        }

        public virtual IEnumerator NextState(UnitCardPresenter card)
        {
            yield break;
        }

        protected void MoveCardToHand(UnitCardPresenter card)
        {
            LeanTween.move(card.childObject, CardHand.Instance.transform.position, 0.2f)
                .setOnComplete(()=> {
                        card.transform.SetParent(CardHand.Instance.transform, false);
                        card.childObject.transform.localPosition = Vector3.zero;
                    }
                );
        }
    }
}
