using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Turns
{
    public class ReceiveUnitsTurn : AbstractTurnState
    {
    
        public override IEnumerator EnterState(TurnManager turnManager, Player player)
        {
            CardHand.Instance.LoadHand(player);
            Events.DisplayUnitSelection(3);
            return base.EnterState(turnManager, player);
        }

        public override IEnumerator EndState(TurnManager turnManager, Player player)
        {
            CardSelector.Instance.SetActive(false);
            turnManager.nextTurnButton.gameObject.SetActive(true);
            turnManager.SwitchTurnState(turnManager.MarketTurn);
            return base.EndState(turnManager, player);
        }
    }
}
