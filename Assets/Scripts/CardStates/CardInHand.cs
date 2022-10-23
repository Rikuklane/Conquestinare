﻿using System.Collections;

namespace CardStates
{
    public class CardInHand: AbstractCardState
    {
        public override IEnumerator CardOnClick(UnitCardPresenter card)
        {
            // TODO somehow place on territory
            return base.CardOnClick(card);
        }

        public override IEnumerator NextState(UnitCardPresenter card)
        {
            card.SwitchState(card.CardInTerritory);
            return base.NextState(card);
        }
    }
}