using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CardStates
{
    public class CardInTerritory: AbstractCardState {
        public override IEnumerator CardOnClick(UnitCardPresenter card)
        {
            if(AttackLogic.instance.isReorganizeTriggered)
            {
                card.TriggerSelected();
            }
            return base.CardOnClick(card);
        }
    }
}