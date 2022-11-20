using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CardStates
{
    public class CardInTerritory: AbstractCardState {
        public override IEnumerator CardOnClick(CardPresenterAbstractLogic card)
        {
            if(AttackLogic.Instance.isReorganizeTriggered)
            {
                // territory has only 1 troop
                if(AttackLogic.Instance.selectedTerritory.TerritoryGraphics.presentUnits.Count == 1)
                {
                    card.ChangeInteractable(false);
                    return base.CardOnClick(card);
                }
                card.TriggerSelected();
                AttackLogic.Instance.selectedTerritory.TerritoryGraphics.CheckSelected();
            }
            return base.CardOnClick(card);
        }
    }
}