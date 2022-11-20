using System.Collections;

namespace CardStates
{
    public class CardInHand: AbstractCardState
    {
        public override IEnumerator CardOnClick(CardPresenterAbstractLogic card)
        {
            CardHand.Instance.NewCardSelected(card);

            // TODO somehow place on territory
            return base.CardOnClick(card);
        }

        public override IEnumerator NextState(CardPresenterAbstractLogic card)
        {
            card.SwitchState(CardStateController.Instance.CardInTerritory);
            return base.NextState(card);
        }
    }
}