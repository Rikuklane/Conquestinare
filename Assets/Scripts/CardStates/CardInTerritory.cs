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
                // territory has only 1 troop
                if(AttackLogic.instance.selectedTerritory.TerritoryGraphics.presentUnits.Count == 1)
                {
                    card.changeInteractable(false);
                    return base.CardOnClick(card);
                }
                card.TriggerSelected();
                AttackLogic.instance.selectedTerritory.TerritoryGraphics.CheckSelected();
            }
            return base.CardOnClick(card);
        }
    }
}