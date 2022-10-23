using System.Collections;

namespace CardStates
{
    public class CardInMarket: AbstractCardState
    {
        public override IEnumerator CardOnClick(UnitCardPresenter card)
        {
            // TODO check if enough gold & subtract
            card.SwitchState(card.CardInHand);
            card.transform.SetParent(CardHand.Instance.transform, false);
            return base.CardOnClick(card);
        }

        public override IEnumerator NextState(UnitCardPresenter card)
        {
            card.SwitchState(card.CardInHand);
            return base.NextState(card);
        }
    }
}