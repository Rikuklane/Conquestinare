using System.Collections;
using UnityEngine;

namespace Turns
{
    public class MarketTurn: AbstractTurnState
    {
        public override IEnumerator EnterState(TurnManager turnManager, Player player)
        {
            AttackLogic.instance.canHover = false;
            Events.DisplayMarketSelection(3);
            return base.EnterState(turnManager, player);
        }

        public override IEnumerator EndState(TurnManager turnManager, Player player)
        {
            CardSelector.Instance.SetActive(false);
            turnManager.SwitchTurnState(turnManager.PlaceUnitsTurn);
            return base.EndState(turnManager, player);
        }
    }
}