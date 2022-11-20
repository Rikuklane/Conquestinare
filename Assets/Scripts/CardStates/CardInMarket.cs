using System.Collections;

namespace CardStates
{
    public class CardInMarket: AbstractCardState
    {
        public override IEnumerator CardOnClick(UnitCardPresenter card)
        {
            var currentPlayer = Events.RequestPlayer();
            var cardCost = card.unitData.cost;
            var currentGold = Events.RequestGold(currentPlayer);
            if (cardCost > currentGold) return base.CardOnClick(card);
            Events.SetGold(currentPlayer, currentGold - cardCost);
            
            NextState(card);
            return base.CardOnClick(card);
        }

        public override IEnumerator NextState(UnitCardPresenter card)
        {
            card.SwitchState(card.CardInHand);
            MoveCardToHand(card);
            return base.NextState(card);
        }
        
        
    }
}