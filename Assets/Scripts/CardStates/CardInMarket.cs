using System.Collections;

namespace CardStates
{
    public class CardInMarket: AbstractCardState
    {
        public override IEnumerator CardOnClick(CardPresenterAbstractLogic card)
        {
            var currentPlayer = Events.RequestPlayer();
            var cardCost = card.CardData.cost;
            var currentGold = Events.RequestGold(currentPlayer);
            if (cardCost > currentGold) return base.CardOnClick(card);
            Events.SetGold(currentPlayer, currentGold - cardCost);
            AudioController.Instance.coin.Play();
            AudioController.Instance.cardChoiceSelect.Play();
            NextState(card);
            return base.CardOnClick(card);
        }

        public override IEnumerator NextState(CardPresenterAbstractLogic card)
        {
            card.SwitchState(CardStateController.Instance.CardInHand);
            MoveCardToHand(card, false);
            return base.NextState(card);
        }

    }
}